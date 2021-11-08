using Domain.Statuses;

namespace Application.Models.Statuses
{
    public class AddStatusModel
    {
        public State State { get; set; }
        public string TicketId { get; set; }
    }
}