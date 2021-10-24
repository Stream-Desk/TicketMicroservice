using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Comments;
using Domain.Comments;
using Domain.Tickets;

namespace Application.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentsCollection _commentsCollection;

        public CommentService(ICommentsCollection commentsCollection)
        {
            _commentsCollection = commentsCollection;
        }
        public async Task<GetCommentModel> CreateComment(LeaveCommentModel model)
        {
            // Validate model
            if (model == null)
            {
                throw new Exception("Comment not Found");
            }
            // Map Comment
            var comment = new Comment
            {
                Text = model.Text,
                TimeStamp = DateTime.Now,
                TicketId = model.TicketId,
            };

            // // Get the Ticket by Id
            // var ticket = await _ticketCollection.GetTicketById(ticketId);
            //
            var newComment = await _commentsCollection.CreateComment(comment);
            
            var response = new GetCommentModel
            {
                Id = newComment.Id,
                Text = newComment.Text,
                TimeStamp = newComment.TimeStamp,
                TicketId = newComment.TicketId,
            };
            
            // // Creating a comment
            // var commentT = new Comment
            // {
            //     Id = response.Id,
            //     Text = response.Text,
            //     TicketId = response.TicketId,
            //     TimeStamp = response.TimeStamp
            // };
            //
            // // Adding the comment to the List of Comments
            // ticket.Comments.Add(commentT);
            //
            // // Calling the Update Method
            // _ticketCollection.UpdateTicket(ticketId,ticket);
            
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
                TimeStamp = result.TimeStamp,
                TicketId = result.TicketId
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
                    TicketId = result.TicketId
                };
                response.Add(model);
            }
            return response;
        }

        public async Task<List<GetCommentModel>> GetCommentsByTicketIdAsync(string ticketId, CancellationToken cancellationToken = default)
        {
            var results = await _commentsCollection.GetCommentByTicketIdAsync(ticketId, cancellationToken);

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
                    TicketId = result.TicketId
                };
                response.Add(model);
            }
            return response;
        }

        public void UpdateComment(string commentId, UpdateCommentModel model)
        {
            if (string.IsNullOrWhiteSpace(commentId))
            {
                throw new Exception("Comment Id doesnt Exist");
            }

            if (model == null)
            {
                throw new Exception("Failed to Find Comment");
            }

            var currentComment = _commentsCollection.GetCommentById(commentId).Result;

            if (currentComment == null)
            {
                throw new Exception("Comment not found");
            }

            currentComment.Text = model.Text;
            currentComment.TimeStamp = model.TimeStamp;
            
            _commentsCollection.UpdateComments(commentId,currentComment);
        }

        public void DeleteCommentById(DeleteCommentModel model)
        {
            if (model == null)
            {
                throw new Exception("Comment Not Found");
            }
            _commentsCollection.DeleteCommentById(model.Id);
        }
    }
}