namespace Application.Models.Comments
{
    public class GetCommentModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string TimeStamp { get; set; } 
        public string TicketId { get; set; }
    }
}