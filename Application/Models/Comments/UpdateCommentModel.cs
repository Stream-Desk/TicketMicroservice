using System;

namespace Application.Models.Comments
{
    public class UpdateCommentModel
    {
        public string Text { get; set; }
        public string TimeStamp { get; set; } = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        public string TicketId { get; set; }
    }
}