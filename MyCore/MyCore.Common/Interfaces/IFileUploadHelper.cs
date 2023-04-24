using Microsoft.AspNetCore.Http;

using MyCore.Common.Helper;

namespace MyCore.Common.Interfaces;

public interface IFileUploadHelper
{
    List<string> SaveImage(List<PicturesUploadModel> ImgList);
    Task<List<string>> ConvertToBase64(List<IFormFile> files);
    Task<string> ConvertToBase64(IFormFile file);
}