using System;

namespace Application.Models.Comments
{
    public class UpdateCommentModel
    {
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; } 
        public string TicketId { get; set; }
        public string UserId { get; set; }
    }
}