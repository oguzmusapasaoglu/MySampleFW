using Microsoft.AspNetCore.Http;
using MyCore.LogManager.ExceptionHandling;
using MyCore.Common.Base;

namespace MyCore.Middlewares;
public class ExceptionMiddleware
{
    private RequestDelegate next;
    public ExceptionMiddleware(RequestDelegate _next)
    {
        next = _next;
    }
    public async Task<ResponseBase<dynamic>> InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case CustomException e:
                    return CreateExceptionResponse<dynamic>(ExceptionTypeEnum.Warn, e.Message);            
                case KnownException ke:
                    return CreateExceptionResponse<dynamic>(ExceptionTypeEnum.Warn, ke.Message);
                default:
                    return CreateExceptionResponse<dynamic>(ExceptionTypeEnum.Warn, exception.Message);
            }
        }
        return ResponseHelper.SuccessResponse<dynamic>("");
    }



    //    private ResponseBase<string> HandleCustomException(CustomException ex)
    //    {
    //        return CreateExceptionResponse<string>(ex.ExceptionType, ex.Message);
    //    }
    //    private ResponseBase<string> HandleKnownException(HttpContext context, KnownException ex)
    //    {
    //        //First, get the incoming request
    //        var reqModel = MiddlewareHelper.FormatRequest(context.Request).Result;
    //        CreateExceptionLog(ex.MethotName, reqModel.RequestData, reqModel.RequestIP, reqModel.RequestUserId, ex.Message, ex.ExceptionType, ex.ExceptionProp);
    //        return CreateExceptionResponse<string>(ex.ExceptionType, ex.Message);
    //    }
    //    private ResponseBase<string> HandleException(HttpContext context, Exception ex)
    //    {
    //        //First, get the incoming request
    //        var reqModel = MiddlewareHelper.FormatRequest(context.Request).Result;
    //        CreateExceptionLog(KnownException.GetMethodName(ex), reqModel.RequestData, reqModel.RequestIP, reqModel.RequestUserId, ExceptionMessageHelper.UnexpectedSystemError, ExceptionTypeEnum.Fattal, ex);
    //        return CreateExceptionResponse<string>(ExceptionTypeEnum.Fattal, ExceptionMessageHelper.UnexpectedSystemError);
    //    }

    private ResponseBase<dynamic> CreateExceptionResponse<T>(ExceptionTypeEnum exceptionType, string exceptionMessage)
    {
        var resultEnum = (exceptionType != ExceptionTypeEnum.Fattal || exceptionType != ExceptionTypeEnum.Error)
            ? ResultEnum.Warning
           : ResultEnum.Error;
        return ResponseHelper.ErrorResponse<dynamic>(exceptionMessage, resultEnum);
    }
    //    private void CreateExceptionLog(string methodName, string requestData,
    //        string requestIP, int requestUserId, string exceptionMessage, ExceptionTypeEnum exceptionType, Exception exceptionProp)
    //    {
    //        ILogServices logServices = provider.GetService<ILogServices>();
    //        logServices.AddExceptionLog(new ExceptionLogModel
    //        {
    //            MethodName = methodName,
    //            RequestData = requestData,
    //            RequestIP = requestIP,
    //            ExceptionMessage = exceptionMessage,
    //            ExceptionType = exceptionType,
    //            ExceptionProp = exceptionProp,
    //            RequestDate = DateTime.Now,
    //            RequestUserId = requestUserId
    //        });
    //    }
}

//public class ExceptionHandlingMiddleware
//{
//    private readonly RequestDelegate next;
//    private IServiceProvider provider;
//    public ExceptionHandlingMiddleware(RequestDelegate _next)//,IServiceProvider _provider)
//    {
//        next = _next;
//        //provider = _provider;
//    }

//    public async Task InvokeAsync(HttpContext httpContext)
//    {
//        try
//        {
//            await next(httpContext);
//        }
//        //catch (CustomException nex)
//        //{
//        //    HandleCustomException(nex);
//        //}
//        //catch (KnownException kex)
//        //{
//        //    HandleKnownException(httpContext, kex);
//        //}
//        catch (Exception exception)
//        {
//            switch (exception)
//            {
//                case CustomException e:
//                    //errorResult.StatusCode = (int)e.StatusCode;
//                    //if (e.ErrorMessages is not null)
//                    //{
//                    //    errorResult.Messages = e.ErrorMessages;
//                    //}
//                    break;
//                case CustomException ne:
//                    //errorResult.StatusCode = (int)HttpStatusCode.NotFound;
//                    break;
//                case KnownException k:
//                    //errorResult.StatusCode = (int)HttpStatusCode.NotFound;
//                    break;

//                default:
//                    //errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
//                    break;
//            }
//        }
//    }
//    //public class ExceptionMiddleware : IMiddleware
//    //{
//    //    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//    //    {
//    //        try
//    //        {
//    //            await next(context);
//    //        }
//    //        catch (Exception exception)
//    //        {
//    //            string errorId = Guid.NewGuid().ToString();
//    //            //LogContext.PushProperty("ErrorId", errorId);
//    //            //LogContext.PushProperty("StackTrace", exception.StackTrace);
//    //            //var errorResult = new ErrorResult
//    //            //{
//    //            //    Source = exception.TargetSite?.DeclaringType?.FullName,
//    //            //    Exception = exception.Message.Trim(),
//    //            //    ErrorId = errorId,
//    //            //    SupportMessage = $"Provide the Error Id: {errorId} to the support team for further analysis."
//    //            //};
//    //            //errorResult.Messages.Add(exception.Message);





//    //            Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
//    //            var response = context.Response;
//    //            if (!response.HasStarted)
//    //            {
//    //                response.ContentType = "application/json";
//    //                response.StatusCode = errorResult.StatusCode;
//    //                await response.WriteAsync(JsonConvert.SerializeObject(errorResult));
//    //            }
//    //            else
//    //            {
//    //                Log.Warning("Can't write error response. Response has already started.");
//    //            }
//    //        }
//    //    }
//    //}
//}
