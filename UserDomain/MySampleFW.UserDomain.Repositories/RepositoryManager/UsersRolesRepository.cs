using System.Linq.Expressions;
using MyCore.Dapper.Interfaces;
using MyCore.Common.ConfigHelper;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.UserDomain.Libraries.Entities;
using MySampleFW.UserDomain.Data.Interfaces;

namespace MySampleFW.UserDomain.Repositories.RepositoryManager;
public class UsersRolesRepository : IUsersRolesRepository
{
    private IDbFactory dbFactory;
    private string connectionString => MainSettingsConfigModelHelper.GetConnection();
    public async Task<UsersRolesEntity> Create(UsersRolesEntity entity, int requestUserId)
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
    public async Task<UsersRolesEntity> Update(UsersRolesEntity entity, int requestUserId)
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
    public async Task<IQueryable<UsersRolesEntity>> GetAllByFilter(Expression<Func<UsersRolesEntity, bool>> filter)
    {
        return dbFactory.GetAllByFilter(connectionString, filter);
    }
    public async Task<UsersRolesEntity> GetSingleByFilter(Expression<Func<UsersRolesEntity, bool>> filter)
    {
        return dbFactory.GetSingleByFilter(connectionString, filter);
    }
    public async Task<IQueryable<UsersRolesEntity>> GetAllActive()
    {
        var result = dbFactory.GetAll<UsersRolesEntity>(connectionString);
        return result.IsNotNullOrEmpty()
            ? new List<UsersRolesEntity>().AsQueryable()
            : result.Where(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
    }
    public async Task<UsersRolesEntity> GetSingleById(int id)
    {
        return dbFactory.GetSingleById<UsersRolesEntity>(connectionString, id);
    }
    public async Task<bool> BulkCreate(List<UsersRolesEntity> request)
    {
        return dbFactory.InsertBulkEntity(connectionString, request);
    }
}
