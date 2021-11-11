using Domain.Statuses;

namespace Application.Models.Statuses
{
    public class GetStatusModel
    {
        public string Id { get; set; }
        public State State { get; set; }
        public string TicketId { get; set; }
    }
}