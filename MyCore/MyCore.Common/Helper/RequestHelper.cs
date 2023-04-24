using MyCore.Common.Base;

namespace MyCore.Common.Helper;

public static class RequestHelper
{
    public static RequestBase<TRequest> CreateServicesRequest<TRequest>(this TRequest request, int requestUserId)
        where TRequest : class
    {
        return new RequestBase<TRequest>
        {
            RequestUserId = requestUserId,
            RequestData = request
        };
    }
}
