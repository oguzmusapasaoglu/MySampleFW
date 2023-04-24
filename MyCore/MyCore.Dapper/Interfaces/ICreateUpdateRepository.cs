using MyCore.Dapper.Base;

namespace MyCore.Dapper.Interfaces
{
    public interface ICreateAndUpdateRepository<TEntity>
        where TEntity : BaseDapperEntity
    {
        Task<TEntity> Create(TEntity request, int requestUserId);
        Task<TEntity> Update(TEntity request, int requestUserId);
    }
}
