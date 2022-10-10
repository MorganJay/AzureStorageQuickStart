using AngularWebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AngularWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly string _container;
        private readonly IBlobStorage _storage;
        private readonly string _connectionString;

        public StorageController(IBlobStorage storage, IConfiguration config)
        {
            _storage = storage;
            _connectionString = config.GetValue<string>("MyConfig:StorageConnection");
            _container = config.GetValue<string>("MyConfig:ContainerName");
        }

        [HttpGet("files")]
        public async Task<List<string>> ListFiles()
        {
            return await _storage.GetAllDocuments(_connectionString, _container);
        }

        [HttpPost("upload")]
        public async Task<bool> InsertFile([FromForm] FileUploadDto request)
        {
            if (request.Asset != null)
            {
                Stream stream = request.Asset.OpenReadStream();
                await _storage.UploadDocument(_connectionString, _container, request.Asset.FileName, stream);
                return true;
            }

            return false;
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile([FromRoute] string fileName)
        {
            var content = await _storage.GetDocument(_connectionString, _container, fileName);
            return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpDelete("{fileName}")]
        public async Task<bool> DeleteFile(string fileName)
        {
            return await _storage.DeleteDocument(_connectionString, _container, fileName);
        }
    }
}
