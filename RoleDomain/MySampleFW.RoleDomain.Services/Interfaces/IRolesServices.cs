using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Models;
using MyCore.Common.Interfaces;

namespace MySampleFW.RoleDomain.Services.Interfaces;
public interface IRolesServices : ICreateOrUpdate<RolesModel, RolesCreateOrUpdateModel>
{
    ResponseBase<RolesModel> ChangeStatus(RequestBase<RolesStatusChangeModel> request);
    ResponseBase<IQueryable<RolesModel>> GetDataByFilter(RolesFilterModel request);
    ResponseBase<RolesModel> GetSingleDataByFilter(RolesFilterModel request);
}