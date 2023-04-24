using System.Linq.Expressions;
using MyCore.Dapper.Interfaces;
using MyCore.Common.ConfigHelper;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.UserDomain.Libraries.Entities;
using MySampleFW.UserDomain.Data.Interfaces;

namespace MySampleFW.UserDomain.Repositories.RepositoryManager;
public class UserInfoRepository : IUserInfoRepository
{
    private IDbFactory dbFactory;
    private string connectionString => MainSettingsConfigModelHelper.GetConnection();
    public UserInfoRepository(IDbFactory _dbFactory)
    {
        dbFactory = _dbFactory;
    }

    public async Task<UserInfoEntity> Create(UserInfoEntity entity, int requestUserId)
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

    public async Task<UserInfoEntity> Update(UserInfoEntity entity, int requestUserId)
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

    public async Task<IQueryable<UserInfoEntity>> GetAllByFilter(Expression<Func<UserInfoEntity, bool>> filter)
    {
        return dbFactory.GetAllByFilter(connectionString, filter);
    }
    public async Task<IQueryable<UserInfoEntity>> GetAllActive()
    {
        var result = dbFactory.GetAllActive<UserInfoEntity>(connectionString);
        return result;
    }
    public async Task<UserInfoEntity> GetSingleById(int userID)
    {
        return dbFactory.GetSingleById<UserInfoEntity>(connectionString, userID);
    }

    public UserInfoEntity? GetUserInfoLogin(string userName, string email)
    {
        var data = dbFactory.GetAll<UserInfoEntity>(connectionString);
        if (data.Any())
        {
            var result = data.FirstOrDefault(q => q.ActivationStatus == (int)ActivationStatusEnum.Active
            || q.UserName.ToLower() == userName.ToLower()
            || q.EMail.ToLower() == email.ToLower());
            return result;
        }
        return null;
    }
}

