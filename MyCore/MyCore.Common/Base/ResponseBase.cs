namespace MyCore.Common.Base;

public class ResponseBase<TResponse> where TResponse : class
{
    public ResultEnum ResultValue { get; set; }
    public string Message { get; set; }
    public List<string> MessageList { get; set; }
    public TResponse Data { get; set; }
}
