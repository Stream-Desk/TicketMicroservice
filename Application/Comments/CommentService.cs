using System.Threading;
using System.Threading.Tasks;
using Application.Models.Comments;
using Domain.Comments;

namespace Application.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentsCollection _commentsCollection;

        public CommentService(ICommentsCollection commentsCollection)
        {
            _commentsCollection = commentsCollection;
        }
        public async Task<LeaveCommentModel> CreateComment(LeaveCommentModel model, CancellationToken cancellationToken = default)
        {

            var comment = new Comment
            {
                Text = model.Text,
                TimeStamp = model.TimeStamp,
                UserID = model.UserID,
                TicketId = model.TicketId
            };

            var newComment = await _commentsCollection.CreateComment(comment, cancellationToken);
            var response = new LeaveCommentModel
            {
                Text = newComment.Text,
                TimeStamp = newComment.TimeStamp,
                UserID = newComment.UserID,
                TicketId = newComment.TicketId
            };
                return response;
        }
    }
}