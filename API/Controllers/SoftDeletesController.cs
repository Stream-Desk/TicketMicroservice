using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models.Tickets;
using Application.Tickets;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoftDeletesController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public SoftDeletesController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        
        // GET: api/<TicketsController>
        [HttpGet]
        public async Task<ActionResult<List<GetTicketModel>>> GetAsync()
        {
            var response = await _ticketService.GetTicketsWithSoftDeleteFalse();
            return Ok(response);
        }
        
        // Soft Delete a Ticket
        // PUT api/<TicketsController>/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Put([FromRoute] string id,[FromBody] DeleteTicketModel model)
        {
            _ticketService.IsSoftDeleted(id, model);
            return NoContent();
        }
    }
}
