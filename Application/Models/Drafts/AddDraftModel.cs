using Domain.Tickets;
using System;

namespace Application.Models.Drafts
{
    public class AddDraftModel
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Priority Priority { get; set; }
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public string Attachment { get; set; }
    }
}
