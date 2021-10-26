using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Attachments;
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
        private readonly IAttachmentService _attachmentService;

        public FilesController(IAttachmentService attachmentService,IFileService fileService)
        {
            _fileService = fileService;
            _attachmentService = attachmentService;
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

        [HttpPost("uploadattachments")]
        public async Task<ActionResult<AttachmentResponse>> UploadAttachmentsAsync(List<IFormFile> files)
        {
            string baseUrl = $"https://{Request.Host.Value}{Request.PathBase.Value}";

            var payload = new AttachmentRequest
            {
                BaseUrl = baseUrl,
                Files = files
            };
            var response = await _attachmentService.UploadAttachmentAsync(payload);

            return Ok(response);
        }
    }
}