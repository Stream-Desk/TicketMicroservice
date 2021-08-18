using System.Threading;
using System.Threading.Tasks;
using Application.Models.Comments;

namespace Application.Comments
{
    public interface ICommentService
    {
        Task<LeaveCommentModel> CreateComment(LeaveCommentModel model, CancellationToken cancellationToken = default);
    }
}