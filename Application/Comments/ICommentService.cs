using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Comments;

namespace Application.Comments
{
    public interface ICommentService
    {
        Task<GetCommentModel> CreateComment(LeaveCommentModel model, CancellationToken cancellationToken = default);
        Task<GetCommentModel> GetCommentById(string commentId, CancellationToken cancellationToken = default);
        Task<List<GetCommentModel>> GetComments(CancellationToken cancellationToken = default); }
}