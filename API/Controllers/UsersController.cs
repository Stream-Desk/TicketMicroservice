using Application.Users;
using Application.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<List<GetUserModel>>> GetAsync()
        {
            var response = await _userService.GetUsers();

            return Ok(response);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserModel>> GetUserByIdAsync([FromRoute] string id)
        {
            var response = await _userService.GetUserById(id);

            return Ok(response);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<GetUserModel>> PostAsync(
            [FromBody] AddUserModel model)
        {
            var response = await _userService.CreateUser(model);

            return Ok(response);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] UpdateUserModel model)
        {
            _userService.UpdateUser(id, model);
            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            _userService.DeleteUserById(new DeleteUserModel { Id = id });
            return NoContent();
        }
    }
}