using MyCore.Common.Base;

public class ResponseHelper
{
    public static ResponseBase<TResult> ErrorResponse<TResult>
        (string erorMessage,
        ResultEnum resultType = ResultEnum.Warning) where TResult : class
    {
        var response = new ResponseBase<TResult>();
        response.Message = erorMessage;
        response.ResultValue = resultType;
        return response;
    }

    public static ResponseBase<TResult> ErrorResponse<TResult>(List<string> erorMessages, ResultEnum resultType = ResultEnum.Warning)
        where TResult : class
    {
        var response = new ResponseBase<TResult>();
        response.MessageList = erorMessages;
        response.ResultValue = resultType;
        return response;
    }

    public static ResponseBase<TResult> SuccessResponse<TResult>(TResult result, ResultEnum resultType = ResultEnum.Success)
    where TResult : class
    {
        var response = new ResponseBase<TResult>();
        response.Data = result;
        response.ResultValue = resultType;
        return response;
    }
}
