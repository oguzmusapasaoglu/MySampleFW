namespace MyCore.Common.Base;

[Serializable]
public class RequestBase<TRequest>
    where TRequest : class
{
    public int RequestUserId { get; set; }
    public TRequest RequestData { get; set; }
}

[Serializable]
public class RequestDynamicBase<dynamic>
{
    public int RequestUserId { get; set; }
    public string RequestData { get; set; }
}
