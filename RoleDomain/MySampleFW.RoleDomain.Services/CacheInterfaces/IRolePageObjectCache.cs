using MyCore.Cache.Interfaces;
using MySampleFW.RoleDomain.Libraries.Models;

namespace MySampleFW.RoleDomain.Services.CacheInterfaces;

public interface IRolePageObjectCache : ICacheManager<RolePageObjectModel>
{
    IQueryable<RolePageObjectModel> GetDataByRoleId(int roleId);
}