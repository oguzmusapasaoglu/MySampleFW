using MyCore.Dapper.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;

namespace MySampleFW.RoleDomain.Repositores.Interfaces;

public interface IRolePageRepository : ICreateAndUpdateRepository<RolePageEntity>
{
    Task<IQueryable<RolePageEntity>> GetAllRolePage();
    Task<IQueryable<RolePageEntity>> GetRolePagesByUserID(int userID);
    Task<RolePageEntity> GetSingleRolePageByID(int id);
}
