using System;
using System.Collections.Generic;
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
        public async Task<GetCommentModel> CreateComment(LeaveCommentModel model, CancellationToken cancellationToken = default)
        {

            var comment = new Comment
            {
                Text = model.Text,
                TimeStamp = model.TimeStamp,
            };

            var newComment = await _commentsCollection.CreateComment(comment, cancellationToken);
            var response = new GetCommentModel
            {
                Text = newComment.Text,
                TimeStamp = newComment.TimeStamp,
            };
                return response;
        }

        public async Task<GetCommentModel> GetCommentById(string commentId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(commentId))
            {
                throw new Exception("Comment empty");
            }

            var result = await _commentsCollection.GetCommentById(commentId, cancellationToken);
            if (result == null)
            {
                return new GetCommentModel();
            }

            var response = new GetCommentModel
            {
                Id = result.Id,
                Text = result.Text,
                TimeStamp = result.TimeStamp
            };
            return response;
        }

        public async Task<List<GetCommentModel>> GetComments(CancellationToken cancellationToken = default)
        {
            var results = await _commentsCollection.GetComments(cancellationToken);
            if (results == null || results.Count < 1)
            {
                return new List<GetCommentModel>();
            }
            var response = new List<GetCommentModel>();

            foreach (var result in results)
            {
                var model = new GetCommentModel
                {
                    Id = result.Id,
                    Text = result.Text,
                    TimeStamp = result.TimeStamp,
                };
                response.Add(model);
            }
            return response;
        }
    }
}