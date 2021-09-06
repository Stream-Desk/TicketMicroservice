using System;
using Domain.Users;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Drafts
{
    public class GetDraftModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public Priority Priority { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public string Attachment { get; set; }
    }
}
