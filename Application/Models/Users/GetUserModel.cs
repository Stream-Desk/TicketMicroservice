using Application.Models.Tickets;
using System;
using System.Collections.Generic;

namespace Application.Models.Users
{
    public class GetUserModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }



        public List<GetUserModel> Users { get; set; } = new List<GetUserModel>();
    }
}

