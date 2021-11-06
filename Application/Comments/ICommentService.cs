using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Comments;

namespace Application.Comments
{
    public interface ICommentService
    {
        Task<GetCommentModel> CreateComment(AddCommentModel model);
        Task<GetCommentModel> GetCommentById(string commentId, CancellationToken cancellationToken = default);
        Task<List<GetCommentModel>> GetComments(CancellationToken cancellationToken = default);
        Task<List<GetCommentModel>> GetCommentsByTicketIdAsync(string ticketId, CancellationToken cancellationToken = default);
        void UpdateComment(string commentId, UpdateCommentModel model);
        void DeleteCommentById(DeleteCommentModel model);
    }
}