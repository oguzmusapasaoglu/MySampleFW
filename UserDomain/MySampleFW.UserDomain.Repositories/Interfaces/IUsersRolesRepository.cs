using MySampleFW.UserDomain.Libraries.Entities;
using System.Linq.Expressions;
using MyCore.Dapper.Interfaces;

namespace MySampleFW.UserDomain.Data.Interfaces;
public interface IUsersRolesRepository : ICreateAndUpdateRepository<UsersRolesEntity>
{
    Task<bool> BulkCreate(List<UsersRolesEntity> request);
    Task<IQueryable<UsersRolesEntity>> GetAllByFilter(Expression<Func<UsersRolesEntity, bool>> filter);
    Task<IQueryable<UsersRolesEntity>> GetAllActive();
    Task<UsersRolesEntity> GetSingleById(int id);
    Task<UsersRolesEntity> GetSingleByFilter(Expression<Func<UsersRolesEntity, bool>> filter);
}