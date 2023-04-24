using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Entities;
using MySampleFW.RoleDomain.Libraries.Models;
using MyCore.Common.Interfaces;

namespace MySampleFW.RoleDomain.Services.Interfaces;
public interface IPagesServices : ICreateOrUpdate<PagesModel, PagesCreateOrUpdateModel>
{
    ResponseBase<PagesModel> ChangeStatus(RequestBase<PagesStatusChangeModel> request);
    ResponseBase<IQueryable<PagesModel>> GetDataByFilter(PagesFilterModel request);
    ResponseBase<PagesModel> GetSingleDataByFilter(PagesFilterModel request);
    Task<IQueryable<UserRolesPagesRepository>> GetPagesByUserID(int userID);
}