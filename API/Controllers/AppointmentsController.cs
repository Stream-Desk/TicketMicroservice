using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers;
using Application.Appointments;
using Application.Models;
using Application.Models.Appointments;
using Application.Models.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
       

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        
        // GET: api/Appoitnments
        [HttpGet]
        public async Task<ActionResult<List<GetAppointmentsModel>>> GetAsync()
        {
            var response = await _appointmentService.GetAppointments();
            return Ok(response);
        }

        // GET: api/Appointments/5
        [HttpGet("{id:length(24)}", Name = "Get")]
        public async Task<ActionResult<GetAppointmentsModel>> GetAppointmentByIdAsync([FromRoute] string id)
        {
            var response = await _appointmentService.GetAppointmentById(id);
            return Ok(response);
        }

        // POST: api/Appoitnments
        [HttpPost]
        public async Task<ActionResult<GetAppointmentsModel>> PostAsync([FromBody] AddAppointmentModel model, [FromForm] MailData mailData)
        {
            var response = await _appointmentService.CreateAppointment(model);
            
            return Ok(response);
        }
        

        // DELETE: api/Appointments/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _appointmentService.CancelAppointment(new CancelAppointmnentModel{Id = id});
            return NoContent();
        }
        
    }
}