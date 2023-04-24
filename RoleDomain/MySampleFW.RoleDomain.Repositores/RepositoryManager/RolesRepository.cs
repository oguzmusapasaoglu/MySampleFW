using System.Linq.Expressions;
using MySampleFW.RoleDomain.Libraries.Entities;
using MyCore.Dapper.Interfaces;
using MyCore.Common.ConfigHelper;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.RoleDomain.Repositores.Interfaces;

namespace MySampleFW.RoleDomain.Repositores.RepositoryManager;

public class RolesRepository : IRolesRepository
{
    private IDbFactory dbFactory;
    private string connectionString => MainSettingsConfigModelHelper.GetConnection();
    public RolesRepository(IDbFactory _dbFactory)
    {
        dbFactory = _dbFactory;
    }

    public async Task<RolesEntity> Create(RolesEntity entity, int requestUserId)
    {
        try
        {
            if (!entity.ID.IsNullOrLessOrEqToZero())
                throw new CustomException(ExceptionMessageHelper.UnauthorizedAccess(requestUserId));

            entity.ActivationStatus = (int)ActivationStatusEnum.Active;
            entity.CreatedBy = requestUserId;
            entity.CreatedDate = DateTime.Now;
            var returnData = dbFactory.InsertEntity(connectionString, entity);
            entity.ID = returnData.Value;
            return entity;
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex, ExceptionMessageHelper.UnexpectedSystemError);
        }
    }
    public async Task<RolesEntity> Update(RolesEntity entity, int requestUserId)
    {
        try
        {
            if (entity.ID.IsNullOrLessOrEqToZero())
                throw new CustomException(ExceptionMessageHelper.UnauthorizedAccess(requestUserId));

            entity.UpdateBy = requestUserId;
            entity.UpdateDate = DateTime.Now;
            dbFactory.UpdateEntity(connectionString, entity);
            return entity;
        }
        catch (Exception ex)
        {
            throw new KnownException(ExceptionTypeEnum.Fattal, ex, ExceptionMessageHelper.UnexpectedSystemError);
        }
    }
    public async Task<IQueryable<RolesEntity>> GetAllByFilter(Expression<Func<RolesEntity, bool>> filter)
    {
        return dbFactory.GetAllByFilter(connectionString, filter);
    }
    public async Task<IQueryable<RolesEntity>> GetAllActive()
    {
        var result = dbFactory.GetAll<RolesEntity>(connectionString);
        return result.IsNotNullOrEmpty()
            ? new List<RolesEntity>().AsQueryable()
            : result.Where(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
    }
    public async Task<RolesEntity> GetSingleById(int id)
    {
        return dbFactory.GetSingleById<RolesEntity>(connectionString, id);
    }
}
