using MyCore.Common.ConfigHelper;
using MyCore.Dapper.Interfaces;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.RoleDomain.Libraries.Entities;
using MySampleFW.RoleDomain.Repositores.Interfaces;

using Dapper;

using System.Text;

namespace MySampleFW.RoleDomain.Repositores.RepositoryManager;
public class RolePageRepository : IRolePageRepository
{
    private IDbFactory dbFactory;
    private string connectionString => MainSettingsConfigModelHelper.GetConnection();
    public RolePageRepository(IDbFactory _dbFactory)
    {
        dbFactory = _dbFactory;
    }
    public async Task<RolePageEntity> Create(RolePageEntity entity, int requestUserId)
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
    public async Task<RolePageEntity> Update(RolePageEntity entity, int requestUserId)
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
    public async Task<IQueryable<RolePageEntity>> GetAllRolePage()
    {
        var sbSQL = new StringBuilder();
        sbSQL.AppendLine(@"SELECT ID, PageID, (SELECT PageName FROM   Pages WHERE (ID = PageID)) AS PageName, 
(SELECT RoleName FROM Roles WHERE (ID = RoleID)) AS RoleName, RoleID, ActivationStatus FROM RolePage 
WHERE ActivationStatus = 1");

        var result = dbFactory.GetData<RolePageEntity>(connectionString, sbSQL);
        return result.IsNotNullOrEmpty()
            ? new List<RolePageEntity>().AsQueryable()
            : result.Where(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
    }
    public async Task<RolePageEntity> GetSingleRolePageByID(int id)
    {
        var sbSQL = new StringBuilder();
        sbSQL.AppendFormat(@"SELECT ID, PageID, (SELECT PageName FROM   Pages WHERE (ID = PageID)) AS PageName, 
(SELECT RoleName FROM Roles WHERE (ID = RoleID)) AS RoleName, RoleID, ActivationStatus FROM RolePage 
WHERE ActivationStatus = 1 AND ID = {0}", id);

        var result = dbFactory.GetSingle<RolePageEntity>(connectionString, sbSQL);
        return result;
    }

    public async Task<IQueryable<RolePageEntity>> GetRolePagesByUserID(int userID)
    {
        var sbSQL = new StringBuilder();
        sbSQL.AppendLine(@"SELECT RolePage.ID, RolePage.PageID, RolePage.RoleID, RolePage.ActivationStatus")
            .AppendLine(" (SELECT PageName FROM Pages WHERE (ID = RolePage.PageID)) AS PageName, ")
            .AppendLine(" (SELECT RoleName FROM Roles WHERE (ID = RolePage.RoleID)) AS RoleName, ")
            .AppendLine(" FROM RolePage INNER JOIN UsersRoles ON RolePage.RoleID = UsersRoles.RoleID ")
            .AppendLine(" WHERE (RolePage.ActivationStatus = 1) AND (UsersRoles.UserID = @UserID)");
        var parm = new DynamicParameters();
        parm.Add("@UserID", userID);
        var result = dbFactory.GetData<RolePageEntity>(connectionString, sbSQL, parm);
        return result;
    }
}
