using MySampleFW.RoleDomain.Libraries.Entities;
using MyCore.Dapper.Interfaces;
using MyCore.Common.ConfigHelper;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.RoleDomain.Repositores.Interfaces;
using System.Linq.Expressions;
using Dapper;
using System.Text;

namespace MySampleFW.RoleDomain.Repositores.RepositoryManager;

public class PagesRepository : IPagesRepository
{
    private IDbFactory dbFactory;
    private string connectionString => MainSettingsConfigModelHelper.GetConnection();

    public PagesRepository(IDbFactory _dbFactory)
    {
        dbFactory = _dbFactory;
    }
    public async Task<PagesEntity> Create(PagesEntity entity, int requestUserId)
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
    public async Task<PagesEntity> Update(PagesEntity entity, int requestUserId)
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
    public async Task<IQueryable<PagesEntity>> GetAllByFilter(Expression<Func<PagesEntity, bool>> filter)
    {
        return dbFactory.GetAllByFilter(connectionString, filter);
    }
    public async Task<IQueryable<PagesEntity>> GetAllActive()
    {
        var result = dbFactory.GetAll<PagesEntity>(connectionString);
        return result.IsNotNullOrEmpty()
            ? new List<PagesEntity>().AsQueryable()
            : result.Where(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
    }
    public async Task<PagesEntity> GetSingleById(int userID)
    {
        return dbFactory.GetSingleById<PagesEntity>(connectionString, userID);
    }
    public async Task<IQueryable<UserRolesPagesRepository>> GetPagesByUserID(int userID)
    {
        var sbSQL = new StringBuilder();
        sbSQL.AppendLine(@"SELECT Pages.ID, Pages.BindPageId, Pages.PageName, Pages.IconName, ")
            .AppendLine(" Pages.PageURL, Pages.PageLevel, Pages.Description, ")
            .AppendLine(" (SELECT RoleName FROM Roles WHERE (ID = RolePage.RoleID)) AS RoleName, ")
            .AppendLine(" UsersRoles.RoleID ")
            .AppendLine(" FROM RolePage ")
            .AppendLine(" INNER JOIN UsersRoles ON RolePage.RoleID = UsersRoles.RoleID ")
            .AppendLine(" INNER JOIN Pages ON RolePage.PageID = Pages.ID ")
            .AppendLine(" WHERE (UsersRoles.UserID = @UserID) AND (Pages.ActivationStatus = 1)");

        var parm = new DynamicParameters();
        parm.Add("@UserID", userID);
        var result = dbFactory.GetData<UserRolesPagesRepository>(connectionString, sbSQL, parm);
        return result;
    }
}
