using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Models;
using MyCore.Common.Interfaces;

namespace MySampleFW.RoleDomain.Services.Interfaces;
public interface IRolePageObjectServices : ICreateOrUpdate< RolePageObjectModel, RolePageObjectCreateOrUpdateModel  >
{
    ResponseBase<IQueryable<RolePageObjectModel>> GetDataByFilter(RolePageObjectFilterModel request);
    ResponseBase<RolePageObjectModel> GetSingleDataByFilter(RolePageObjectFilterModel request);
}