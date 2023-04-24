using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Models;
using MySampleFW.RoleDomain.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController, Authorize]
public class PagesController : ControllerBase
{
    private IPagesServices services;
    public PagesController(IPagesServices _services)
    {
        services = _services;
    }

    [HttpPost("CreateOrUpdate")]
    public ResponseBase<PagesModel> CreateOrUpdate(RequestBase<PagesCreateOrUpdateModel> request)
    {
        return services.CreateOrUpdate(request);
    }

    [HttpPost("SearchData")]
    public ResponseBase<IQueryable<PagesModel>> SearchData(RequestBase<PagesFilterModel> request)
    {
        return services.GetDataByFilter(request.RequestData);
    }

    [HttpPost("SingleData")]
    public ResponseBase<PagesModel> SingleData(RequestBase<PagesFilterModel> request)
    {
        return services.GetSingleDataByFilter(request.RequestData);
    }
}
