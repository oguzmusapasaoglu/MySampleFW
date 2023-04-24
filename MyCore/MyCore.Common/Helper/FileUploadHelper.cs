using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using MyCore.Common.Interfaces;

namespace MyCore.Common.Helper;
public class FileUploadHelper : IFileUploadHelper
{
    private IWebHostEnvironment webHostEnvironment;

    public FileUploadHelper(IWebHostEnvironment _webHostEnvironment)
    {
        webHostEnvironment = _webHostEnvironment;
    }
    public List<string> SaveImage(List<PicturesUploadModel> ImgList)
    {
        try
        {
            List<string> imgNames = new List<string>();
            string contentRootPath = webHostEnvironment.ContentRootPath;

            var imgPath = Path.Combine(contentRootPath, "wwwroot\\Files");

            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);
            foreach (var imgItem in ImgList)
            {
                var imgName = (imgItem.FileName.IsNullOrEmpty())
                    ? Guid.NewGuid() + ".jpg"
                    : imgItem.FileName;
                byte[] imageBytes = Convert.FromBase64String(imgItem.Base64String);
                File.WriteAllBytes(imgPath + "\\" + imgName, imageBytes);
                imgNames.Add(imgName);
            }
            return imgNames;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<List<string>> ConvertToBase64(List<IFormFile> files)
    {
        List<string> result = new List<string>();
        foreach (var file in files)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                result.Add(Convert.ToBase64String(bytes));
            }
        }
        return result;
    }
    public async Task<string> ConvertToBase64(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            return Convert.ToBase64String(bytes);
        }
    }
}
public class PicturesUploadModel
{
    public string FileName { get; set; }
    public string Base64String { get; set; }
}
