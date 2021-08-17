using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Tickets;
using Domain.Tickets;

namespace Application.Tickets
{
    public class TicketsService : ITicketService
    {
        private readonly ITicketCollection _ticketCollection;

        public TicketsService(ITicketCollection ticketCollection)
        {
            _ticketCollection = ticketCollection;
        }
        public async Task<List<GetTicketModel>> GetTickets(CancellationToken cancellationToken = default)
        {
            var searchResults = await _ticketCollection.GetTickets(cancellationToken);
            if (searchResults == null || searchResults.Count < 1)
            {
                return new List<GetTicketModel>();
            }

            var result = new List<GetTicketModel>();

            foreach (var searchResult in searchResults)
            {
                var model = new GetTicketModel
                {
                    Id = searchResult.Id,
                    Description = searchResult.Description,
                    Summary = searchResult.Summary,
                    Priority = searchResult.Priority,
                    SubmitDate = searchResult.SubmitDate,
                    // User = searchResult.User,
                };
                result.Add(model);
            }
            return result;
        }

        public async Task<GetTicketModel> GetTicketById(string ticketId, CancellationToken cancellationToken = default)
        {
           // validate
           if (string.IsNullOrWhiteSpace(ticketId))
           {
               throw new Exception("Ticket not Found");
           }

           var search = await _ticketCollection.GetTicketById(ticketId, cancellationToken);
           if (search == null)
           {
               return new GetTicketModel();
           }

           var result = new GetTicketModel
           {
               Id = search.Id,
               Description = search.Description,
               Summary = search.Summary,
               Priority = search.Priority,
               SubmitDate = search.SubmitDate,
               Attachment = search.Attachment,
           };
           return result;
        }

        public async Task<GetTicketModel> CreateTicket(AddTicketModel model, CancellationToken cancellationToken = default)
        {
            // validate model

            if (model == null)
            {
                throw new Exception("Ticket details empty");
            }
            
            // Map model to domain Entity
            var ticket = new Ticket
            {
                Description = model.Description,
                Summary = model.Summary,
                Priority = model.Priority,
                SubmitDate = model.SubmitDate,
                Attachment = model.Attachment,
            };

            var search = await _ticketCollection.CreateTicket(ticket, cancellationToken);
            var result = new GetTicketModel
            {
                Description = search.Description,
                Priority = search.Priority,
                Summary = search.Summary,
                SubmitDate = search.SubmitDate,
                Attachment = search.Attachment
            };

            return result;
        }

        public void UpdateTicket(string ticketId, UpdateTicketModel model)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(ticketId))
            {
                throw new Exception("Ticket Id does not exist");
            }

            if (model == null)
            {
                throw new Exception("Failed to find ticket");
            }
            
            // get ticket by Id
            var ticket = _ticketCollection.GetTicketById(ticketId).Result;

            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            ticket.Summary = model.Summary;
            ticket.Description = model.Description;
            ticket.Priority = model.Priority;
            ticket.SubmitDate = model.SubmitDate;
            
           _ticketCollection.UpdateTicket(ticketId, ticket);
        }
        public void DeleteTicketById(DeleteTicketModel model)
        {
            // validation
            if (model == null)
            {
                throw new Exception("Ticket not found");
            }
            _ticketCollection.DeleteTicketById(model.Id);
        }
    }
}