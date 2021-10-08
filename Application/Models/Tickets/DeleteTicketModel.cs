using System;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Tickets
{
    public class DeleteTicketModel
    {
        public string Id { get; set; }
       
    }
}