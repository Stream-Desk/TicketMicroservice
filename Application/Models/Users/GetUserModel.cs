using Application.Models.Tickets;
using System.Collections.Generic;

namespace Application.Models.Users
{
    public class GetUserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public List<GetTicketModel> Ticket { get; set; } = new List<GetTicketModel>();
    }
}

