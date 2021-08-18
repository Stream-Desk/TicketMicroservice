using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Comments;
using Application.Models.Comments;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

            public CommentsController(ICommentService commentService)
            {
                _commentService = commentService;
            }
            // GET: api/Comments
            [HttpGet]
            public async Task<ActionResult<List<GetCommentModel>>> GetAsync()
            {
                var response = await _commentService.GetComments();
                return Ok(response);
            }

            // GET: api/Comments/5
            [HttpGet("{id}")]
            public async Task<ActionResult<GetCommentModel>> GetCommentByIdAsync([FromRoute] string id)
            {
                var response = await _commentService.GetCommentById(id);
                return Ok(response);
            }

            // POST: api/Comments
            [HttpPost]
            public async Task<ActionResult<GetCommentModel>> PostAsync([FromBody] LeaveCommentModel model)
            {
                var response = await _commentService.CreateComment(model);
                return Ok(response);
            }
    }
}