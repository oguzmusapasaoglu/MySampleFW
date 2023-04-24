using MyCore.Dapper.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;

using System.Linq.Expressions;

namespace MySampleFW.RoleDomain.Repositores.Interfaces;
public interface IPageObjectRepository : ICreateAndUpdateRepository<PageObjectEntity>
{
    Task<IQueryable<PageObjectEntity>> GetAllByFilter(Expression<Func<PageObjectEntity, bool>> filter);
    Task<IQueryable<PageObjectEntity>> GetAllActive();
    Task<PageObjectEntity> GetSingleById(int id);
}