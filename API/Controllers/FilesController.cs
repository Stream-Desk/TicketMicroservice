using System;
using System.IO;
using System.Threading.Tasks;
using Application.Files;
using Application.Models.Files;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FilesController(IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }

        // POST: api/Files/Upload
        [HttpPost]
        public async Task <ActionResult<DownloadFileModel>> UploadToFileSystem(IFormFile file)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var basePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files");
            var fileName = Path.GetFileNameWithoutExtension(file.FileName.Replace(" ", "_"));
            var filePath = Path.Combine(basePath, fileName);
            var extension = Path.GetExtension(file.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
                var fileModel = new AddFileModel
                {
                    CreatedOn = DateTime.Now.ToLocalTime(),
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    FilePath = filePath,
                };
              
                var search = await _fileService.UploadFile(fileModel);
                
                var result = new DownloadFileModel()
                {
                    FileId = search.FileId,
                    FileUrl = $"{baseUrl}/api/Files/{search.FileId}"
                };
                return Ok(result);
            }
            throw new Exception("Upload Failed");
        }

        // GET: api/Files/Download
        [HttpGet("{fileId:Length(24)}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var file = await _fileService.DownloadImage(fileId);
            if (file == null) return null;
            var memory = new MemoryStream();
            await using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }
        

        // // DELETE: api/Files/5
        // [HttpDelete("{id=Length:24}")]
        // public Task<IActionResult> DeleteFile(DeleteFileModel model)
        // {
        //     var file = _fileService.DeleteFile(model);
        //     if (file == null) return null;
        //     if (System.IO.File.Exists(file.FilePath))
        //     {
        //         System.IO.File.Delete(file.FilePath);
        //     }
        // }
    }
}