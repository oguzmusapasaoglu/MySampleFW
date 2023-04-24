using System.Linq.Expressions;
using MySampleFW.RoleDomain.Libraries.Entities;
using MyCore.Dapper.Interfaces;
using MyCore.Common.ConfigHelper;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.RoleDomain.Repositores.Interfaces;

namespace MySampleFW.RoleDomain.Repositores.RepositoryManager;
public class RolePageObjectRepository : IRolePageObjectRepository
{
    private IDbFactory dbFactory;
    private string connectionString => MainSettingsConfigModelHelper.GetConnection();

    public async Task<RolePageObjectEntity> Create(RolePageObjectEntity entity, int requestUserId)
    {
        try
        {
            if (!entity.ID.IsNullOrLessOrEqToZero())
                throw new CustomException(ExceptionMessageHelper.UnauthorizedAccess(requestUserId));

            entity.ActivationStatus = (int)ActivationStatusEnum.Active;
            var returnData = dbFactory.InsertEntity(connectionString, entity);
            entity.ID = returnData.Value;
            return entity;
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex, ExceptionMessageHelper.UnexpectedSystemError);
        }
    }
    public async Task<RolePageObjectEntity> Update(RolePageObjectEntity entity, int requestUserId)
    {
        try
        {
            if (entity.ID.IsNullOrLessOrEqToZero())
                throw new CustomException(ExceptionMessageHelper.UnauthorizedAccess(requestUserId));
            dbFactory.UpdateEntity(connectionString, entity);
            return entity;
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex, ExceptionMessageHelper.UnexpectedSystemError);
        }
    }
    public async Task<IQueryable<RolePageObjectEntity>> GetAllByFilter(Expression<Func<RolePageObjectEntity, bool>> filter)
    {
        return dbFactory.GetAllByFilter(connectionString, filter);
    }
    public async Task<IQueryable<RolePageObjectEntity>> GetAllActive()
    {
        var result = dbFactory.GetAll<RolePageObjectEntity>(connectionString);
        return result.DataIsNullOrEmpty()
            ? new List<RolePageObjectEntity>().AsQueryable()
            : result.Where(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
    }
    public async Task<RolePageObjectEntity> GetSingleById(int id)
    {
        return dbFactory.GetSingleById<RolePageObjectEntity>(connectionString, id);
    }
}
