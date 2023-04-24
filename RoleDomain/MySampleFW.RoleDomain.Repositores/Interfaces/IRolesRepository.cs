using MyCore.Dapper.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;

using System.Linq.Expressions;

namespace MySampleFW.RoleDomain.Repositores.Interfaces;

public interface IRolesRepository : ICreateAndUpdateRepository<RolesEntity>
{
    Task<RolesEntity> GetSingleById(int ID);
    Task<IQueryable<RolesEntity>> GetAllByFilter(Expression<Func<RolesEntity, bool>> filter);
    Task<IQueryable<RolesEntity>> GetAllActive();
}
