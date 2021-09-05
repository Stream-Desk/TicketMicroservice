using Application.Registrations;
using Application.Models.Registrations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // GET: api/<RegistrationsController>
        [HttpGet]
        public async Task<ActionResult<List<GetRegistrationModel>>> GetAsync()
        {
            var response = await _registrationService.GetRegistrations();

            return Ok(response);
        }

        // GET api/<RegistrationsController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<GetRegistrationModel>> GetRegistrationByIdAsync([FromRoute] string id)
        {
            var response = await _registrationService.GetRegistrationById(id);

            return Ok(response);
        }

        // POST api/<RegistrationsController>
        [HttpPost]
        public async Task<ActionResult<GetRegistrationModel>> PostAsync(
            [FromBody] AddRegistrationModel model)
        {
            var response = await _registrationService.CreateRegistration(model);

            return Ok(response);
        }

        // PUT api/<RegistrationsController>/5
        [HttpPut("{id:length(24)}")]
        public IActionResult Put([FromRoute] string id, [FromBody] UpdateRegistrationModel model)
        {
            _registrationService.UpdateRegistration(id, model);
            return NoContent();
        }

        // DELETE api/<RegistrationsController>/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _registrationService.DeleteRegistrationById(new DeleteRegistrationModel { Id = id });
            return NoContent();
        }
    }
}

