using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Comments
{
    public interface ICommentsCollection
    {
        Task<Comment> CreateComment(Comment comment, string ticketId);
        Task<List<Comment>> GetComments(CancellationToken cancellationToken = default);
        Task<Comment> GetCommentById(string commentId, CancellationToken cancellationToken = default);
        Task<List<Comment>> GetCommentByTicketIdAsync(string ticketId, CancellationToken cancellationToken = default);
        void UpdateComments(string commentId, Comment comment);
        void DeleteCommentById(string commentId);
    }
}