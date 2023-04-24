using MyCore.Common.Base;
using MySampleFW.UserDomain.Libraries.Models;
using MySampleFW.UserDomain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private IUserInfoServices services;
    public UserController(IUserInfoServices _services)
    {
        services = _services;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public ResponseBase<UserLoginResponseModel> UserLogin([FromBody] UserLoginRequestModel model)
    {
        return services.UserLogin(model);
    }

    [HttpPost("Create")]
    public ResponseBase<UserInfoModel> CreateUserInfo([FromBody] RequestBase<UserInfoCreateModel> request)
    {
        return services.Create(request);
    }
    [HttpPost("Update")]
    public ResponseBase<UserInfoModel> UpdateUserInfo([FromBody] RequestBase<UserInfoUpdateModel> request)
    {
        return services.Update(request);
    }

    [HttpPost("ChangePassword")]
    public ResponseBase<UserInfoModel> ChangePassword([FromBody] RequestBase<UserInfoChangePasswordModel> request)
    {
        return services.ChangePassword(request);
    }

    [HttpPost("ChangeStatus")]
    public ResponseBase<UserInfoModel> ChangeStatus([FromBody] RequestBase<UserInfoStatusChangeModel> request)
    {
        return services.ChangeStatus(request);
    }
    [HttpPost("SearchData")]
    public ResponseBase<IQueryable<UserInfoListModel>> SearchData([FromBody] RequestBase<UserInfoFilterModel> request)
    {
        return services.GetDataByFilter(request.RequestData);
    }
    [HttpPost("SingleData")]
    public ResponseBase<UserInfoModel> SingleData([FromBody] RequestBase<UserInfoFilterModel> request)
    {
        return services.GetSingleDataByFilter(request.RequestData);
    }
}
