using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Models;
using MyCore.Common.Interfaces;

namespace MySampleFW.RoleDomain.Services.Interfaces;
public interface IPageObjectServices : ICreateOrUpdate<PageObjectModel, PageObjectCreateOrUpdateModel>
{
    ResponseBase<PageObjectModel> ChangeStatus(RequestBase<PageObjectStatusChangeModel> request);
    ResponseBase<IQueryable<PageObjectModel>> GetDataByFilter(PageObjectFilterModel request);
    ResponseBase<PageObjectModel> GetSingleDataByFilter(PageObjectFilterModel request);
}