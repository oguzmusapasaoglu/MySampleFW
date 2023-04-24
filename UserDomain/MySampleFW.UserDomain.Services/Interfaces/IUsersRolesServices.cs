using MyCore.Common.Base;
using MySampleFW.UserDomain.Libraries.Models;
using MyCore.Common.Interfaces;

namespace MySampleFW.UserDomain.Services.Interfaces;
public interface IUsersRolesServices : ICreateOrUpdate<UsersRolesModel, UsersRolesCreateOrUpdateModel>
{
    ResponseBase<dynamic> BulkCreate(RequestBase<List<UsersRolesBulkCreateModel>> request);
    ResponseBase<IQueryable<UsersRolesModel>> GetDataByFilter(UsersRolesFilterModel request);
    ResponseBase<IQueryable<UsersRolesModel>> GetDataByRoleID(UsersRolesFilterModel request);
    ResponseBase<IQueryable<UsersRolesModel>> GetDataByUserID(UsersRolesFilterModel request);
}
