using Application.Tickets;
using Application.Models.Tickets;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        
        [HttpGet("search")]
        public async Task<ActionResult<List<GetTicketModel>>> SearchTickets(string searchTerm)
        {
            var response = _ticketService.SearchTickets(searchTerm);
            return Ok(response);
        }
            
        // GET api/<TicketsController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<GetTicketModel>> GetTicketByIdAsync([FromRoute] string id)
        {
            var response = await _ticketService.GetTicketById(id);
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

        // DELETE api/<TicketsController>/5
        [HttpDelete("Labo/{id:length(24)}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _ticketService.DeleteTicketById(new DeleteTicketModel { Id = id });
            return NoContent();
        }
        
        // Soft Delete a Ticket
        // PUT api/<TicketsController>/5
        [HttpPut("BO/Delete/{id:length(24)}")]
        public IActionResult Put([FromRoute] string id,[FromBody] DeleteTicketModel model)
        {
            _ticketService.IsSoftDeleted(id, model);
            return NoContent();
        }
        
        // GET: api/<TicketsController>
        [HttpGet("BO/All")]
        public async Task<ActionResult<List<GetTicketModel>>> GetAsync()
        {
            var response = await _ticketService.GetTicketsWithSoftDeleteFalse();
            return Ok(response);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<List<GetTicketModel>>> Pagination(int page)
        {
            var response = await _ticketService.Pagination(page);
            return Ok(response);
        }
    }
}
