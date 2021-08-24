using System;

namespace Application.Models.Comments
{
    public class GetCommentModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string TicketId { get; set; }
        public string UserId { get; set; }
    }
}