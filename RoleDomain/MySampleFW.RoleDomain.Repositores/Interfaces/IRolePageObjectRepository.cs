using MyCore.Dapper.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;

using System.Linq.Expressions;

namespace MySampleFW.RoleDomain.Repositores.Interfaces;
public interface IRolePageObjectRepository : ICreateAndUpdateRepository<RolePageObjectEntity>
{
    Task<IQueryable<RolePageObjectEntity>> GetAllByFilter(Expression<Func<RolePageObjectEntity, bool>> filter);
    Task<IQueryable<RolePageObjectEntity>> GetAllActive();
    Task<RolePageObjectEntity> GetSingleById(int id);
}