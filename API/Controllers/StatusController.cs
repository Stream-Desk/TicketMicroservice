using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models.Statuses;
using Application.Statuses;
using Domain.Statuses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : Controller
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpPost]
        public async Task<ActionResult<GetStatusModel>> Post(AddStatusModel model)
        {
            var response = await _statusService.CreateStatus(model);
            return response;
        }

        [HttpGet("{id:Length(24)}")]
        public async Task<ActionResult<GetStatusModel>> GetAsync(string id)
        {
            var response = await _statusService.GetStatusById(id);
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetStatusModel>>> GetAllAsync()
        {
            var response = await _statusService.GetAllStatuses();
            return response;
        }
    }
}