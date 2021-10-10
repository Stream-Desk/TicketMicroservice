using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Attachments;
using Application.Files;
using Application.Models.Files;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ContentDispositionHeaderValue = System.Net.Http.Headers.ContentDispositionHeaderValue;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAttachmentService _attachmentService;

        public FilesController(IFileService fileService, IWebHostEnvironment webHostEnvironment,
            IAttachmentService attachmentService)
        {
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _attachmentService = attachmentService;
        }

        // POST: api/Files/Upload
        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                    var basePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files");
                    var fileName = Guid.NewGuid()+Path.GetFileNameWithoutExtension(file.FileName.Replace(" ", "_"));
                    var filePath = Path.Combine(basePath, fileName);
                    var extension = Path.GetExtension(file.FileName);
                    
                    if (System.IO.File.Exists(filePath))
                    {
                        throw new Exception("Upload Failed");
                    }

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileModel = new AddFileModel
                    {
                        CreatedOn = DateTime.Now,
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
            }
            
            catch (Exception)
            {
                throw;
            }
            return Ok();
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
        

        [HttpGet("ListFiles")]
        public async Task<ActionResult<List<DownloadFileModel>>> ListFilesAsync()
        {
            var response = await _fileService.ListImages();
            return Ok(response);
        }
        
        [HttpPost("upload"), DisableRequestSizeLimit]
        public  IActionResult Upload (IFormFile file)
        {
            try
            {
                // var file = Request.Form.Files[0];
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files");

                if (file.Length > 0)
                {
                    string baseURL = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(filePath, file.FileName);
                    var dbPath = baseURL + "/api/Files/" + fileName;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new
                    {
                        dbPath
                    });

                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }

        [HttpPost("uploadattachments")]
        public async Task<ActionResult<AttachmentResponse>> UploadAttachmentsAsync(List<IFormFile> files, CancellationToken cancellationToken = default)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";

            var request = new AttachmentRequest()
            {
                BaseUrl = baseUrl,
                Files = files
            };
            var response = await _attachmentService.UploadAttachmentAsync(request, cancellationToken);
            
            return Ok(response);
        }
        
    }
}