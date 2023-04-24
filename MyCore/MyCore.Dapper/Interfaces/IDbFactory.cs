using MyCore.Dapper.Base;

using Dapper;

using System.Linq.Expressions;
using System.Text;

namespace MyCore.Dapper.Interfaces;

public interface IDbFactory
{
    int? InsertEntity<TEntity>(string connectionString, TEntity entity)
    where TEntity : BaseDapperEntity;
    bool UpdateEntity<TEntity>(string connectionString, TEntity entity)
        where TEntity : BaseDapperEntity;
    bool  InsertBulkEntity<TEntity>(string connectionString, List<TEntity> entities)
        where TEntity : BaseDapperEntity;
    TEntity GetSingleById<TEntity>(string connectionString, int id)
     where TEntity : BaseDapperEntity;
    IQueryable<TEntity> GetAll<TEntity>(string connectionString)
        where TEntity : BaseDapperEntity;
    IQueryable<TEntity> GetAllActive<TEntity>(string connectionString) 
        where TEntity : BaseDapperEntity;
    IQueryable<TEntity> GetAllByFilter<TEntity>(string connectionString, Expression<Func<TEntity, bool>> filter)
        where TEntity : BaseDapperEntity;
    TEntity GetSingleByFilter<TEntity>(string connectionString, Expression<Func<TEntity, bool>> filter)
        where TEntity : BaseDapperEntity;
    IQueryable<TEntity> GetData<TEntity>(string connectionString, StringBuilder queryScript, DynamicParameters parameters);
    IQueryable<TEntity> GetData<TEntity>(string connectionString, StringBuilder queryScript);
    TEntity GetSingle<TEntity>(string connectionString, StringBuilder queryScript);
}