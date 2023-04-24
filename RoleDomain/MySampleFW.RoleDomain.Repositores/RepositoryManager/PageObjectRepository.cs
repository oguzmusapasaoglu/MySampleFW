using System.Linq.Expressions;
using MySampleFW.RoleDomain.Libraries.Entities;
using MyCore.Dapper.Interfaces;
using MyCore.Common.ConfigHelper;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.RoleDomain.Repositores.Interfaces;

namespace MySampleFW.RoleDomain.Repositores.RepositoryManager;
public class PageObjectRepository : IPageObjectRepository
{
    private IDbFactory dbFactory;
    private string connectionString => MainSettingsConfigModelHelper.GetConnection();
  
    public PageObjectRepository(IDbFactory _dbFactory)
    {
        dbFactory = _dbFactory;
    }

    public async Task<PageObjectEntity> Create(PageObjectEntity entity, int requestUserId)
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

    public async Task<PageObjectEntity> Update(PageObjectEntity entity, int requestUserId)
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

    public async Task<IQueryable<PageObjectEntity>> GetAllByFilter(Expression<Func<PageObjectEntity, bool>> filter)
    {
        return dbFactory.GetAllByFilter(connectionString, filter);
    }
    public async Task<IQueryable<PageObjectEntity>> GetAllActive()
    {
        var result = dbFactory.GetAll<PageObjectEntity>(connectionString);
        return result.IsNotNullOrEmpty()
            ? new List<PageObjectEntity>().AsQueryable()
            : result.Where(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
    }
    public async Task<PageObjectEntity> GetSingleById(int userID)
    {
        return dbFactory.GetSingleById<PageObjectEntity>(connectionString, userID);
    }
}
