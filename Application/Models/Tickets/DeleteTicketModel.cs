using System.ComponentModel;

namespace Application.Models.Tickets
{
    public class DeleteTicketModel
    {
        public string Id { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}