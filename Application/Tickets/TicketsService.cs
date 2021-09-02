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
        
        // Banks BO Ticket List
        public async Task<List<GetTicketModel>> GetTicketsWithSoftDeleteFalse(CancellationToken cancellationToken = default)
        {
            var searches = await _ticketCollection.GetTickets(cancellationToken);
            if (searches == null || searches.Count < 1)
            {
                return new List<GetTicketModel>();
            }
            
            var result = new List<GetTicketModel>();
            
            foreach (var search in searches)
            {
                var model = new GetTicketModel
                {
                    Id = search.Id,
                    Description = search.Description,
                    Summary = search.Summary,
                    Priority = search.Priority,
                    Status = search.Status,
                    Category = search.Category,
                    SubmitDate = search.SubmitDate,
                    User = search.User,
                };
                result.Add(model);
            }
            return result;
        }
        
        // Laboremus Ticket List
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
                    Status = searchResult.Status,
                    Category = searchResult.Category,
                    SubmitDate = searchResult.SubmitDate,
                    User = searchResult.User,
                    IsDeleted = searchResult.IsDeleted
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
               Category = search.Category,
               Priority = search.Priority,
               SubmitDate = search.SubmitDate,
               Status = search.Status,
               User = search.User,
               IsDeleted = search.IsDeleted
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
                Category = model.Category,
                Priority = model.Priority,
                SubmitDate = DateTime.Now.ToLocalTime(),
                Status = model.Status,
            };

            var search = await _ticketCollection.CreateTicket(ticket, cancellationToken);
            var result = new GetTicketModel
            {
                Id = search.Id,
                Description = search.Description,
                Priority = search.Priority,
                Summary = search.Summary,
                Category = search.Category,
                SubmitDate = DateTime.Now.ToLocalTime(),
                Status = search.Status,
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
            var currentTicket = _ticketCollection.GetTicketById(ticketId).Result;
             
            if (currentTicket == null)
            {
                throw new Exception("Ticket not found");
            }

            currentTicket.Summary = model.Summary;
            currentTicket.Description = model.Description;
            currentTicket.Priority = model.Priority;
            currentTicket.Category = model.Category;
            currentTicket.Status = model.Status;
            currentTicket.SubmitDate = DateTime.Now.ToLocalTime();

            _ticketCollection.UpdateTicket(ticketId, currentTicket);
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
        
        // BO Delete
        public void IsSoftDeleted(string ticketId, DeleteTicketModel model)
        {
           // Validation of Deleted Entity
           if (model == null)
           {
               throw new Exception("Ticket not found");
           }
           var softDeletedTicket = _ticketCollection.GetTicketById(ticketId).Result;
           softDeletedTicket.IsDeleted = true;
           _ticketCollection.IsSoftDeleted(ticketId,softDeletedTicket);
        }
    }
}