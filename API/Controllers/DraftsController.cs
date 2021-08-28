using System;
using Application.Drafts;
using Application.Models.Drafts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftsController : ControllerBase
    {
        private readonly IDraftService _draftService;
        private readonly IFileService _fileService;

        public DraftsController(IDraftService draftService, IFileService fileService)
        {
            _draftService = draftService;
            _fileService = fileService;
        }

        // GET: api/<TicketsController>
        [HttpGet]
        public async Task<ActionResult<List<GetDraftModel>>> GetAsync()
        {
            var response = await _draftService.GetDrafts();
            return Ok(response);
        }

        // GET api/<TicketsController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<GetDraftModel>> GetDraftByIdAsync([FromRoute] string id)
        {
            var response = await _draftService.GetDraftById(id);
            return Ok(response);
        }

        // POST api/<DraftsController>
        [HttpPost]
        public async Task<ActionResult<GetDraftModel>> PostAsync(
            [FromBody] AddDraftModel model)
        {
            var response = await _draftService.CreateDraft(model);

            return Ok(response);
        }
        // PUT api/<DraftsController>/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Put([FromRoute] string id, [FromBody] UpdateDraftModel model)
        {
            _draftService.UpdateDraft(id, model);
            return NoContent();
        }

        // DELETE api/<DraftsController>/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _draftService.DeleteDraftById(new DeleteDraftModel { Id = id });
            return NoContent();
        }


        // Upload and Download files
        // Upload File
        [HttpPost(nameof(Upload))]

        public IActionResult Upload([Required] List<IFormFile> formFiles)
        {
            try
            {
                _fileService.UploadFile(formFiles);
                return Ok(new
                {
                    formFiles.Count,
                    formFilesSize = _fileService.SizeConverter(formFiles.Sum(f => f.Length))
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Download File
        [HttpGet(nameof(Download))]
        public IActionResult Download()
        {

            try
            {
                var (fileType, archiveData, archiveName) = _fileService.DownloadFiles();

                return File(archiveData, fileType, archiveName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

