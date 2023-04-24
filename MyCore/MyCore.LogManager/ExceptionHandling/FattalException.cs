namespace MyCore.LogManager.ExceptionHandling;
public class FattalException : KnownException
{
    public FattalException(ExceptionTypeEnum exceptionType, Exception exception, string exceptionMessage)
        : base(exceptionType, exception, exceptionMessage)
    {
        ExceptionProp = exception;
        MethotName = GetMethodName(exception);
        Message = exceptionMessage;
        ExceptionType = exceptionType;
    }
}
