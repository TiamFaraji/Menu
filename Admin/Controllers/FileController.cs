using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _storagePath;
        public FileController(IConfiguration configuration)
        {
            _storagePath = configuration["FileUpload:StoragePath"];
        }

        [HttpGet("GetFile")]
        public IActionResult DownloadFile(string filename)
        {
            var fullpath = Path.Combine(_storagePath, filename);

            if (!System.IO.File.Exists(fullpath))
            {
                return NotFound("File Not Found");
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullpath);
            var contentType = "application/octet-stream";

            return PhysicalFile(fullpath, contentType, filename);
        }
    }
}
