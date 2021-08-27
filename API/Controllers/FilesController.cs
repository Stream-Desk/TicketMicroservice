using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Files;
using Application.Models.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // POST: api/Files
        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "/Files");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileModel = new AddFileModel()
                    {
                        CreatedOn = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        FilePath = filePath
                    };
                    await _fileService.UploadFile(fileModel);
                }
                else
                {
                    throw new Exception("Upload Failed");
                }

            }
            return Ok("Upload Successful");
        }

        // PUT: api/Files/5
        [HttpGet("{id:Length(24)}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var file = await _fileService.DownloadImage(id);
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
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
        //     context.FilesOnFileSystem.Remove(file);
        //     context.SaveChanges();
        // }
    }
}
