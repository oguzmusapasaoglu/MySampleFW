using MyCore.Dapper.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;

using System.Linq.Expressions;
namespace MySampleFW.RoleDomain.Repositores.Interfaces;
public interface IPagesRepository : ICreateAndUpdateRepository<PagesEntity>
{
    Task<PagesEntity> GetSingleById(int userID);
    Task<IQueryable<PagesEntity>> GetAllByFilter(Expression<Func<PagesEntity, bool>> filter);
    Task<IQueryable<PagesEntity>> GetAllActive();
    Task<IQueryable<UserRolesPagesRepository>> GetPagesByUserID(int userID);
}