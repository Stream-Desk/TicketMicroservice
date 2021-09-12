using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Tickets;
using Domain.Tickets;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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
            var searches = await _ticketCollection.GetTicketsWithSoftDeleteFalse(cancellationToken);
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
                    IsModified = search.IsModified,
                    Closed = search.Closed,
                    ClosureDateTime = search.ClosureDateTime
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
                    TicketNumber = searchResult.TicketNumber,
                    Summary = searchResult.Summary,
                    Priority = searchResult.Priority,
                    Status = searchResult.Status,
                    Category = searchResult.Category,
                    SubmitDate = searchResult.SubmitDate,
                    IsDeleted = searchResult.IsDeleted,
                    IsModified = searchResult.IsModified,
                    Closed = searchResult.Closed,
                    ClosureDateTime = searchResult.ClosureDateTime
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
               TicketNumber = search.TicketNumber,
               Summary = search.Summary,
               Category = search.Category,
               Priority = search.Priority,
               SubmitDate = search.SubmitDate,
               Status = search.Status,
               IsDeleted = search.IsDeleted,
               IsModified = search.IsModified,
               ModifiedAt = search.ModifiedAt,
               Closed = search.Closed,
               ClosureDateTime = search.ClosureDateTime
               // User = search.User
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
                TicketNumber = model.TicketNumber,
                Summary = model.Summary,
                Category = model.Category,
                Priority = model.Priority,
                SubmitDate = DateTime.Now.ToLocalTime(),
                Status = model.Status,
                IsDeleted = model.IsDeleted,
                IsModified = model.IsModified,
            };

            var search = await _ticketCollection.CreateTicket(ticket, cancellationToken);
            var result = new GetTicketModel
            {
                Id = search.Id,
                Description = search.Description,
                TicketNumber = search.TicketNumber,
                Priority = search.Priority,
                Summary = search.Summary,
                Category = search.Category,
                SubmitDate = search.SubmitDate,
                Status = search.Status,
                IsDeleted = search.IsDeleted,
                IsModified = search.IsModified
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
            currentTicket.IsModified = true;
            currentTicket.ModifiedAt = DateTime.Now.ToLocalTime();
            currentTicket.Closed = false || true;
            currentTicket.ClosureDateTime = model.ClosureDateTime;
            
            if (model.Closed == true)
            {
                currentTicket.ClosureDateTime = DateTime.Now.ToLocalTime();
                currentTicket.Status = Status.Resolved;
            }
            
            else if (model.IsModified == true)
            {
                currentTicket.Status = Status.Open;
            }
                
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

        public async Task<List<GetTicketModel>> SearchTickets(string searchTerm, CancellationToken cancellationToken = default)
        {
            var searchResults = await _ticketCollection.SearchTicket(searchTerm, cancellationToken);
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
                    TicketNumber = searchResult.TicketNumber,
                    Summary = searchResult.Summary,
                    Priority = searchResult.Priority,
                    Status = searchResult.Status,
                    Category = searchResult.Category,
                    SubmitDate = searchResult.SubmitDate,
                    IsDeleted = searchResult.IsDeleted,
                    IsModified = searchResult.IsModified,
                    Closed = searchResult.Closed,
                    ClosureDateTime = searchResult.ClosureDateTime
                };
                result.Add(model);
            }
            return result;
        }
    }
}