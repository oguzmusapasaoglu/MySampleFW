using System.Text;
using MyCore.LogManager.ExceptionHandling;
using MyCore.LogManager.Models;
using MyCore.Common.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using MyCore.TokenManager;
using MySampleFW.RoleDomain.Services.Interfaces;

namespace MyCore.Middlewares.Helper;

public class MiddlewareHelper
{
    internal static async Task<ResponseBase<ReqResLogModel>> FormatRequest(HttpRequest request)
    {
        //This line allows us to set the reader for the request back at the beginning of its stream.
        request.EnableBuffering();
        //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        //...Then we copy the entire request stream into the new buffer.
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        //We convert the byte[] into a string using UTF8 encoding...
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        if (bodyAsText.IsNullOrEmpty())
            return ResponseHelper.ErrorResponse<ReqResLogModel>(ExceptionMessageHelper.JsonParseException, ResultEnum.Warning);

        var dynamicObject = JsonConvert.DeserializeObject<dynamic>(bodyAsText)!;
        int requestUserId = (int)dynamicObject.RequestUserId;
        string requestData = dynamicObject.RequestData.ToString();

        if (requestUserId.IsNullOrLessOrEqToZero())
            return ResponseHelper.ErrorResponse<ReqResLogModel>(ExceptionMessageHelper.ParseFieldError("RequestUserId"), ResultEnum.Warning);

        var result = new ReqResLogModel
        {
            RequestUserId = requestUserId,
            ServicesName = request.Path,
            RequestData = requestData,
            RequestDate = DateTime.Now
        };
        return ResponseHelper.SuccessResponse(result);
    }
    internal static async Task<ReqResLogModel> FormatResponse(ReqResLogModel reqResLogModel, HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        string text = await new StreamReader(response.Body).ReadToEndAsync();
        //response.Body.Seek(0, SeekOrigin.Begin);

        var buffer = new byte[Convert.ToInt32(response.ContentLength)];
        await response.Body.ReadAsync(buffer, 0, buffer.Length);
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        //var requestHeader = JsonSerializer.Deserialize<ResponseBase<dynamic>>(bodyAsText);

        reqResLogModel.ResponseMessage = text;
        reqResLogModel.ResponseDate = DateTime.Now;
        reqResLogModel.ResponseData = text;
        reqResLogModel.ResponseTotalTime = reqResLogModel.ResponseDate - reqResLogModel.RequestDate;
        return reqResLogModel;
    }
    internal static async Task<int?> ControlToken(HttpRequest httpRequest, int requestUserID)
    {
        string authValue = httpRequest.Headers["Authorization"];
        return TokenHelper.ControlToken(authValue, requestUserID);
    }

    internal static bool AuthorizationControl(IServiceProvider provider, int requestUserId, string servicesName)
    {
        IAuthorizationControlServices authorizationServices;
        authorizationServices = provider.GetService<IAuthorizationControlServices>();
        if (!authorizationServices.AuthorizationControlByUser(requestUserId, servicesName))
            throw new CustomException(ExceptionMessageHelper.UnauthorizedAccess(servicesName));
        return true;
    }
}
