using System;
using Application.Tickets;
using Application.Models.Tickets;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using API.Services;
using Domain.Tickets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IFileService _fileService;

        public TicketsController(ITicketService ticketService, IFileService fileService)
        {
            _ticketService = ticketService;
            _fileService = fileService;
        }

        // GET: api/<TicketsController>
        [HttpGet]
        public async Task<ActionResult<List<GetTicketModel>>> GetAsync()
        {
            var response = await _ticketService.GetTickets();

            return Ok(response);
        }

        // GET api/<TicketsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketModel>> GetTicketByIdAsync([FromRoute] string id)
        {
            var response = await _ticketService.GetTicketById(id);

            return Ok(response);
        }

        // POST api/<TicketsController>
        [HttpPost]
        public async Task<ActionResult<GetTicketModel>> PostAsync(
            [FromBody] AddTicketModel model)
        {
            var response = await _ticketService.CreateTicket(model);
            return Ok(response);
        }
        // PUT api/<TicketsController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] UpdateTicketModel model)
        {
            _ticketService.UpdateTicket(id, model);

            return NoContent();
        }

        // DELETE api/<TicketsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _ticketService.DeleteTicketById(new DeleteTicketModel { Id = id });

            return NoContent();
        }
        
        // Upload and Download files
        // Upload File
        [HttpPost(nameof(Upload))]
        public IActionResult Upload([Required] List<IFormFile> formFiles, [Required] string subDirectory)
        {
            try
            {
                _fileService.UploadFile(formFiles, subDirectory);
                return Ok(new
                {
                    formFiles.Count, formFilesSize = _fileService.SizeConverter(formFiles.Sum(f => f.Length))
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // Download File
        [HttpGet(nameof(Download))]  
        public IActionResult Download([Required]string subDirectory)  
        {  
  
            try  
            {  
                var (fileType, archiveData, archiveName) = _fileService.DownloadFiles(subDirectory);  
  
                return File(archiveData, fileType, archiveName);  
            }  
            catch (Exception e)  
            {  
                return BadRequest(e.Message);  
            }  
  
        }
    }
}
