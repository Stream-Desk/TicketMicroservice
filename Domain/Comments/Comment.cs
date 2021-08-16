using System;

namespace Domain.Comments
{
    public class Comment
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        // public User user {get;set;}
        // public string TicketId { get; set; }
    }
}