using MyCore.Common.Base;
using MySampleFW.UserDomain.Libraries.Models;
using MySampleFW.UserDomain.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController, Authorize]
public class UserRolesController : ControllerBase
{
    private IUsersRolesServices services;
    public UserRolesController(IUsersRolesServices _services)
    {
        services = _services;
    }

    [HttpPost("CreateOrUpdate")]
    public ResponseBase<UsersRolesModel> CreateOrUpdate([FromBody] RequestBase<UsersRolesCreateOrUpdateModel> request)
    {
        return services.CreateOrUpdate(request);
    }
    [HttpPost("BulkCreate")]
    public ResponseBase<dynamic> BulkCreate([FromBody] RequestBase<List<UsersRolesBulkCreateModel>> request)
    {
        return services.BulkCreate(request);
    }
    [HttpPost("SearchData")]
    public ResponseBase<IQueryable<UsersRolesModel>> GetDataByFilter([FromBody] RequestBase<UsersRolesFilterModel> request)
    {
        return services.GetDataByFilter(request.RequestData);
    }
    [HttpPost("GetDataByUserID")]
    public ResponseBase<IQueryable<UsersRolesModel>> GetDataByUserID([FromBody] RequestBase<UsersRolesFilterModel> request)
    {
        return services.GetDataByUserID(request.RequestData);
    }

    [HttpPost("GetDataByRoleID")]
    public ResponseBase<IQueryable<UsersRolesModel>> GetDataByRoleID([FromBody] RequestBase<UsersRolesFilterModel> request)
    {
        return services.GetDataByRoleID(request.RequestData);
    }

}
