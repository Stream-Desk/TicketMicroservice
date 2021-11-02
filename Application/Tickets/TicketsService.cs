using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Mail;
using Application.Models.Tickets;
using Domain.Tickets;
using Infrastracture;
using Microsoft.Extensions.DependencyInjection;
using Application.Models.Mail;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace Application.Tickets
{
    public class TicketsService : ITicketService
    {
        private readonly ITicketCollection _ticketCollection;
        private readonly IMailService _mailService;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly object SendEmail;

        public TicketsService(
            ITicketCollection ticketCollection,
            IServiceScopeFactory scopeFactory,
            IBackgroundTaskQueue backgroundTaskQueue,
            IMailService mailService)
        {
            _ticketCollection = ticketCollection;
            _backgroundTaskQueue = backgroundTaskQueue;
            _mailService = mailService;
            _scopeFactory = scopeFactory;
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
                    Name = search.Name,
                    Summary = search.Summary,
                    Priority = search.Priority,
                    Status = search.Status,
                    Category = search.Category,
                    SubmitDate = search.SubmitDate,
                    IsModified = search.IsModified,
                    Closed = search.Closed,
                    ClosureDateTime = search.ClosureDateTime,
                    TicketNumber = search.TicketNumber,
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
                    Name = searchResult.Name,
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
               Name = search.Name,
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
               ClosureDateTime = search.ClosureDateTime,
               FileUrls = search.FileUrls,
               Comments = search.Comments
           };
           
           return result;
        }

        public async Task<GetTicketModel> GetTicketByIdLaboremus(string ticketId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(ticketId))
            {
                throw new Exception("Ticket not Found");
            }

            var search = await _ticketCollection.GetTicketByIdLaboremus(ticketId, cancellationToken);
            
            if (search == null)
            {
                return new GetTicketModel();
            }

            var result = new GetTicketModel()
            {
                Id = search.Id,
                Name = search.Name,
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
                ClosureDateTime = search.ClosureDateTime,
                FileUrls = search.FileUrls,
                Comments = search.Comments
            };
            
             _ticketCollection.UpdateTicket(search.Id, new Ticket()
            {
                Status = Status.Pending,
                IsModified = true
            });

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
            switch (model.Category)
            {
                case Category.Bug:
                    model.Priority = Priority.High;
                    break;
                case Category.Login:
                    model.Priority = Priority.High;
                    break;
                case Category.Uploads:
                    model.Priority = Priority.Medium;
                    break;
                case Category.Other:
                    model.Priority = Priority.Low;
                    break;
                case Category.FreezingScreen:
                    model.Priority = Priority.High;
                    break;  
                default:
                    model.Priority = Priority.Low;
                    break;
            }
            
            var ticket = new Ticket
            {
                Name = model.Name,
                Summary = model.Summary,
                Description = model.Description,
                Category = model.Category,
                Priority = model.Priority, 
                SubmitDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"),
                Status = Status.Open,
                IsDeleted = model.IsDeleted,
                IsModified = model.IsModified,
                FileUrls = model.FileUrls
            };
            
            var search = await _ticketCollection.CreateTicket(ticket, cancellationToken);

            var result = new GetTicketModel
            {
                Id = search.Id,
                TicketNumber = search.TicketNumber,
                Name = search.Name,
                Description = search.Description,
                Category = search.Category,
                Priority = search.Priority,
                Summary = search.Summary,
                SubmitDate = search.SubmitDate,
                Status = search.Status,
                IsDeleted = search.IsDeleted,
                IsModified = search.IsModified,
                FileUrls = search.FileUrls,
            };

            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async (stoppingToken) =>
            {
                var scope = _scopeFactory.CreateScope();

                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                mailService.SendEmail(new MailData
                {
                    EmailBody = "Hello Catherine, your ticket has been received, you will receive feedback shortly",
                    EmailSubject = "Ticket Received",
                    EmailToId = "catherinececilia22@gmail.com",
                    EmailToName = "cathy"
                });

            });

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
            
           
            // Category to March Priority
            
            switch (currentTicket.Category)
            {
                case Category.Bug:
                    model.Priority = Priority.High;
                    break;
                case Category.Login:
                    model.Priority = Priority.High;
                    break;
                case Category.Uploads:
                    model.Priority = Priority.Medium;
                    break;
                case Category.Other:
                    model.Priority = Priority.Low;
                    break;
                case Category.FreezingScreen:
                    model.Priority = Priority.High;
                    break;  
                default:
                    model.Priority = Priority.Low;
                    break;
            }

            currentTicket.Summary = model.Summary;
            currentTicket.Name = model.Name;
            currentTicket.Description = model.Description;
            currentTicket.Category = model.Category;
            currentTicket.Priority = model.Priority;
            currentTicket.Status = model.Status;
            currentTicket.IsModified = true;
            currentTicket.ModifiedAt = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            currentTicket.Closed = model.Closed;
            currentTicket.ClosureDateTime = model.ClosureDateTime;
            currentTicket.Comments = model.Comments;
            currentTicket.FileUrls = model.FileUrls;

            // Change Status to Modified when Edited
            if (currentTicket.IsModified == true)
            {
                currentTicket.Status = Status.Pending;
            }
            
            if (currentTicket.Closed == true)
            {
                currentTicket.Status = Status.Resolved;
                model.ClosureDateTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            }
            
            else
            {
                model.Status = Status.Open;
            }
            
            _ticketCollection.UpdateTicket(ticketId, currentTicket);
        }

        public void CloseTicket(string ticketId, UpdateTicketModel model)
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

            currentTicket.Closed = true;
            currentTicket.Status = Status.Resolved;
            currentTicket.ClosureDateTime = model.ClosureDateTime;
            
            _ticketCollection.CloseTicket(ticketId,currentTicket);
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
        public void IsSoftDeleted(DeleteTicketModel model)
        {
            if (model == null)
            {
                throw new Exception("Ticket not Found");
            }
            var softDeletedTicket = _ticketCollection.GetTicketById(model.Id).Result;
            softDeletedTicket.IsDeleted = true;
          
            _ticketCollection.IsSoftDeleted(model.Id,softDeletedTicket);
        }
    }
}
