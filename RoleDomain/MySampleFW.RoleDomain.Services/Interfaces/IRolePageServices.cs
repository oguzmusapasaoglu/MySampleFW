using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Models;
using MyCore.Common.Interfaces;

namespace MySampleFW.RoleDomain.Services.Interfaces
{
    public interface IRolePageServices : ICreateOrUpdate<RolePageListModel, RolePageModel>
    {
        Task<ResponseBase<IQueryable<RolePageListModel>>> GetDataByFilter(RolePageModel request);
        Task<IQueryable<RolePageListModel>> GetRolePagesByRoleID(int roleID);
        Task<IQueryable<RolePageListModel>> GetRolePagesByPageID(int pageID);
        Task<ResponseBase<RolePageListModel>> GetSingleDataByID(int id);
        Task<IQueryable<RolePageListModel>> GetRolePagesByUserID(int userID);
    }
}
