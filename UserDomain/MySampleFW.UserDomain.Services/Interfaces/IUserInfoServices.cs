using MyCore.Common.Base;
using MySampleFW.UserDomain.Libraries.Models;

namespace MySampleFW.UserDomain.Services.Interfaces;
public interface IUserInfoServices
{
    ResponseBase<UserInfoModel> Create(RequestBase<UserInfoCreateModel> request);
    ResponseBase<UserInfoModel> Update(RequestBase<UserInfoUpdateModel> request);
    ResponseBase<UserInfoModel> ChangeStatus(RequestBase<UserInfoStatusChangeModel> request);
    ResponseBase<UserInfoModel> ChangePassword(RequestBase<UserInfoChangePasswordModel> request);
    ResponseBase<IQueryable<UserInfoListModel>> GetDataByFilter(UserInfoFilterModel request);
    ResponseBase<UserInfoModel> GetSingleDataByFilter(UserInfoFilterModel request);
    ResponseBase<UserLoginResponseModel> UserLogin(UserLoginRequestModel request);
}