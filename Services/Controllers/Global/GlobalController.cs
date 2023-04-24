using Microsoft.AspNetCore.Mvc;

using MyCore.Common.Base;
using MyCore.Common.Helper;
using MyCore.Common.Interfaces;

namespace CRPAppProject.Services.Controllers.Global;

[Route("[controller]")]
[ApiController]
public class GlobalController : ControllerBase
{
    private IFileUploadHelper fileUploadHelper;

    public GlobalController(
        IFileUploadHelper _fileUploadHelper)
    {
        fileUploadHelper = _fileUploadHelper;
    }

    [HttpPost("UploadWorkPictures")]
    public List<string> UploadWorkPictures([FromBody] List<PicturesUploadModel> upFile)
    {
        var result = fileUploadHelper.SaveImage(upFile);
        return result;
    }

}
