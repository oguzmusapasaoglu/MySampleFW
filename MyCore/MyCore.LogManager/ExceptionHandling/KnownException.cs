using System.Diagnostics;

namespace MyCore.LogManager.ExceptionHandling;
public class KnownException : Exception
{
    public string MethotName { get; set; }
    public ExceptionTypeEnum ExceptionType { get; set; }
    public string Message { get; set; }
    public Exception ExceptionProp { get; set; }
    public KnownException(ExceptionTypeEnum exceptionType, Exception exception, string message)
        : base(message, exception)
    {
        ExceptionType = exceptionType;
        Message = message;
        ExceptionProp = exception;
        MethotName = GetMethodName(exception);
    }
    public KnownException(ExceptionTypeEnum exceptionType, Exception exception)
    {
        ExceptionType = exceptionType;
        Message = exception.Message;
        ExceptionProp = exception;
        MethotName = GetMethodName(exception);
    }
    public static string GetMethodName(Exception exception)
    {
        var trace = new StackTrace(exception).GetFrames().Select(q => q.GetMethod()).FirstOrDefault();
        return (trace.IsNullOrEmpty())
            ? string.Empty
            : trace.DeclaringType.FullName + "." + trace.Name;
    }
}