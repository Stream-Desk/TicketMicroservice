using System;

namespace Application.Models.Registrations
{
    public class UpdateRegistrationModel
    {
        

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TelNumber { get; set; }
        public DateTime SubmitDate { get; set; }


    }
}