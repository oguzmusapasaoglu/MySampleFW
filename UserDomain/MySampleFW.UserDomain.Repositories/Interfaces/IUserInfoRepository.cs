using MySampleFW.UserDomain.Libraries.Entities;
using System.Linq.Expressions;
using MyCore.Dapper.Interfaces;

namespace MySampleFW.UserDomain.Data.Interfaces;
public interface IUserInfoRepository : ICreateAndUpdateRepository<UserInfoEntity>
{
    Task<UserInfoEntity> GetSingleById(int userID);
    UserInfoEntity? GetUserInfoLogin(string userName, string email);
    Task<IQueryable<UserInfoEntity>> GetAllByFilter(Expression<Func<UserInfoEntity, bool>> filter);
    Task<IQueryable<UserInfoEntity>> GetAllActive();
}