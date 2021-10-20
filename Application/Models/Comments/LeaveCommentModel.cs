using System;

namespace Application.Models.Comments
{
    public class LeaveCommentModel
    {
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string TicketId { get; set; }
    }
}