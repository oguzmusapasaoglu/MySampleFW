using MyCore.Common.Base;

namespace MyCore.Common.Interfaces;

public interface ICreateOrUpdate<TResponse, TRequest>
    where TRequest : BaseModel
    where TResponse : BaseModel
{
    ResponseBase<TResponse> CreateOrUpdate(RequestBase<TRequest> request);
}
