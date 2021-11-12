using Application.Tickets;
using Application.Models.Tickets;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Tickets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        
        // GET: api/<TicketsController>
        [HttpGet("Labo/All")]
        public async Task<ActionResult<List<GetTicketModel>>> GetAllAsync()
        {
            var response = await _ticketService.GetTickets();
            return Ok(response);
        }
        
        // GET: api/<TicketsController>
        [HttpGet("BO/All")]
        public async Task<ActionResult<List<GetTicketModel>>> GetAsync()
        {
            var response = await _ticketService.GetTicketsWithSoftDeleteFalse();
            return Ok(response);
        }

        // GET api/<TicketsController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<GetTicketModel>> GetTicketByIdAsync([FromRoute] string id)
        {
            var response = await _ticketService.GetTicketById(id);
            return Ok(response);
        }

        [HttpGet("editlabo/{id:length(24)}")]
        public async Task<ActionResult<GetTicketModel>> GetTicketByIdLaboremus([FromRoute] string id)
        {
            var response = await _ticketService.GetTicketByIdLaboremus(id);
            return Ok(response);
        }

        // POST api/<TicketsController>
        [HttpPost]
        public async Task<ActionResult<GetTicketModel>> PostAsync([FromBody] AddTicketModel model)
        {
            var response = await _ticketService.CreateTicket(model);
            return Ok(response);
        }
        
        // PUT api/<TicketsController>/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Put([FromRoute] string id, [FromBody] UpdateTicketModel model)
        {
            _ticketService.UpdateTicket(id, model);
            return NoContent();
        }
        
        // PUT api/<TicketsController>/5
        [HttpPost("Close/{id:Length(24)}")]
        public IActionResult CloseTicket([FromRoute] string id)
        {
            _ticketService.CloseTicket(new UpdateTicketModel()
            {
                Id = id,
                Closed = true
            });
            return NoContent();
        }
        
        // PUT api/<TicketsController>/5
        [HttpPut("EditStatus/{id:length(24)}")]
        public IActionResult EditStatus([FromRoute] string id, UpdateTicketModel model )
        {
            _ticketService.UpdateTicketStatus(id, model);
            return NoContent();
        }

        // Assign Ticket
        [HttpPost("Assignticket/{id:length(24)}")]
        public IActionResult Assign([FromRoute] string id)
        {
            _ticketService.AssignTicket(new UpdateTicketModel()
            {
                Id = id,
                IsAssigned = true,
                Status = Status.InProgress
            });
            
            return NoContent();
        }
        
        // DELETE api/<TicketsController>/5
        [HttpDelete("Labo/{id:length(24)}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _ticketService.DeleteTicketById(new DeleteTicketModel { Id = id });
            return NoContent();
        }
        
        // Soft Delete a Ticket
        // DELETE api/<TicketsController>/5
        [HttpDelete("BO/Delete/{id:length(24)}")]
        public IActionResult SoftDelete([FromRoute] string id)
        {
            _ticketService.IsSoftDeleted(new DeleteTicketModel
            {
                Id = id,
                IsDeleted = true
            });
            return NoContent();
        }
    }
}
