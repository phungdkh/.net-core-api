using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using HVS.Api.Core.Business.Filters;
using HVS.Api.Core.Common.Helpers;

namespace HVS.Api.Controllers
{
    [Route("api/media")]
    [EnableCors("CorsPolicy")]
    public class MediaController : Controller
    {
        [HttpPost]
        [CustomAuthorize]
        public async Task<IActionResult> UploadFile(string folder, string fileName, IFormFile file)
        {
            var t1 = Task.Run(() => FileHelper.SaveFile(folder, fileName, file));

            await Task.WhenAll(t1);

            var extension = Path.GetExtension(file.FileName);

            return Ok(Url.Action("GetFile", "Media", new { folder = folder, fileName = string.Format("{0}{1}", fileName, extension) }));
        }

        [HttpGet("{folder}/{fileName}")]
        public async Task<IActionResult> GetFile(string folder, string fileName)
        {
            var t1 = Task.Run(() => FileHelper.GetFile(folder, fileName));

            await Task.WhenAll(t1);

            var fileStream = System.IO.File.OpenRead(t1.Result);

            string contentType = GetMimeType(fileName);

            return base.File(fileStream, contentType);
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}