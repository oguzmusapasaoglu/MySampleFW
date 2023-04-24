using MyCore.LogManager.Models;

using Microsoft.AspNetCore.Http;
using MyCore.Middlewares.Helper;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MyCore.LogManager.Services;

namespace MyCore.Middlewares;

public class RequestResponseMiddleware
{
    private readonly RequestDelegate next;
    private IServiceProvider provider;
    public RequestResponseMiddleware(IServiceProvider _provider, RequestDelegate _next)
    {
        next = _next;
        provider = _provider;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.ToString().Contains("Login") || context.Request.Path.ToString().Contains("Global"))
        {
            await next(context);
            return;
        }

        var watch = new Stopwatch();
        watch.Start();
        //First, get the incoming request
        var req = await MiddlewareHelper.FormatRequest(context.Request);
        if (req.ResultValue != ResultEnum.Success)
            return;

        //Control token & requserid
        if (!MiddlewareHelper.ControlToken(context.Request, req.Data.RequestUserId).Result.IsNotNullOrEmpty())
            return;

        if (!MiddlewareHelper.AuthorizationControl(provider, req.Data.RequestUserId, req.Data.ServicesName))
            return;

        await next(context);
        //Copy a pointer to the original response body stream
        var originalBodyStream = context.Response.Body;
        //Create a new memory stream...
        using (var responseBody = new MemoryStream())
        {
            //...and use that for the temporary response body
            context.Response.Body = responseBody;
            //Continue down the Middleware pipeline, eventually returning to this class
            await next(context);
            watch.Stop();
            //Format the response from the server
            var response = await MiddlewareHelper.FormatResponse(req.Data, context.Response);
            //TODO: Save log to chosen datastore
            AddReqResLogData(req.Data);
            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream).ConfigureAwait(false);
        }
    }

    async void AddReqResLogData(ReqResLogModel reqResLogModel)
    {
        ILogServices logServices = provider.GetService<ILogServices>();
        logServices.AddResponseLog(reqResLogModel);
    }
}
