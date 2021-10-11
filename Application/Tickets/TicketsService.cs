using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Comments;
using Application.Models.Files;
using Application.Models.Tickets;
using Application.Service;
using Domain.Tickets;
using Infrastracture;
using Microsoft.Extensions.DependencyInjection;
using Application.Models.Mail;
using Domain.Files;

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
                    TicketNumber = search.TicketNumber
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
               Description = search.Description,
               TicketNumber = search.TicketNumber,
               Name = search.Name,
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
               Attachments = new List<DownloadFileModel>(),
               Comments = new List<GetCommentModel>()
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
                //TicketNumber = model.TicketNumber,
                Name = model.Name,
                Summary = model.Summary,
                Category = model.Category,
                Priority = model.Priority,
                SubmitDate = DateTime.Now.Date,
                Status = Status.Open,
                IsDeleted = model.IsDeleted,
                IsModified = model.IsModified,
                Attachments = new List<File>(),
            };

            var search = await _ticketCollection.CreateTicket(ticket, cancellationToken);
            
            switch (search.Category)
            {
                case Category.Bug:
                    search.Priority = Priority.High;
                    break;
                case Category.Login:
                    search.Priority = Priority.High;
                    break;
                case Category.Uploads:
                    search.Priority = Priority.Medium;
                    break;
                case Category.Other:
                    search.Priority = Priority.Low;
                    break;
                case Category.FreezingScreen:
                    search.Priority = Priority.High;
                    break;  
                default:
                    search.Priority = Priority.Low;
                    break;
            }
            


            var result = new GetTicketModel
            {
                Id = search.Id,
                TicketNumber = search.TicketNumber,
                Name = search.Name,
                Description = search.Description,
                Priority = search.Priority,
                Summary = search.Summary,
                Category = search.Category,
                SubmitDate = search.SubmitDate,
                Status = Status.Open,
                IsDeleted = search.IsDeleted,
                IsModified = search.IsModified,
                Attachments = new List<DownloadFileModel>(),
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

            //currentTicket.TicketNumber = model.TicketNumber;
            currentTicket.Summary = model.Summary;
            currentTicket.Name = model.Name;
            currentTicket.Description = model.Description;
            currentTicket.Priority = model.Priority;
            currentTicket.Category = model.Category;
            currentTicket.Status = model.Status;
            currentTicket.IsModified = true;
            currentTicket.ModifiedAt = DateTime.Now;
            currentTicket.Closed = false || true;
            currentTicket.ClosureDateTime = model.ClosureDateTime;
            currentTicket.Attachments = new List<File>();

            if (model.Closed == true)
            {
                currentTicket.ClosureDateTime = DateTime.Now;
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
    }
}
