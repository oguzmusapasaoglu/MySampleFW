using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Models;
using MySampleFW.RoleDomain.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("[controller]")]
[ApiController, Authorize]
public class RolePageObjectController : ControllerBase
{
    private IRolePageObjectServices services;
    public RolePageObjectController(IRolePageObjectServices _services)
    {
        services = _services;
    }

    [HttpPost("CreateOrUpdate")]
    public ResponseBase<RolePageObjectModel> CreateOrUpdate(RequestBase<RolePageObjectCreateOrUpdateModel> request)
    {
        return services.CreateOrUpdate(request);
    }

    [HttpPost("SearchData")]
    public ResponseBase<IQueryable<RolePageObjectModel>> SearchData(RequestBase<RolePageObjectFilterModel> request)
    {
        return services.GetDataByFilter(request.RequestData);
    }

    [HttpPost("SingleData")]
    public ResponseBase<RolePageObjectModel> SingleData(RequestBase<RolePageObjectFilterModel> request)
    {
        return services.GetSingleDataByFilter(request.RequestData);
    }
}
