using System;
using System.Collections.Generic;
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
        private readonly IAttachmentService _attachmentService;

        public FilesController(IFileService fileService, IWebHostEnvironment webHostEnvironment, IAttachmentService attachmentService)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _attachmentService = attachmentService;
        }

        // POST: api/Files/Upload
        [HttpPost]
        public async Task<ActionResult> UploadToFileSystem(IFormFile file)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var basePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files");
            var fileName = Guid.NewGuid()+Path.GetFileNameWithoutExtension(file.FileName.Replace(" ", "_"));
            var filePath = Path.Combine(basePath, fileName);
            var extension = Path.GetExtension(file.FileName);
           
            if (file == null)
            {
                 throw new Exception("Upload Failed");
            }
           
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

        [HttpPost("UploadAttachment")]
        public async Task<ActionResult<FileResponse>> UploadAttachmentAsync(List<IFormFile> files)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";

            var payload = new FileRequest()
            {
                BaseUrl = baseUrl,
                Files = files
            };

            var response = await _attachmentService.UploadFile(payload);

            return Ok(response);
        }
    }
}