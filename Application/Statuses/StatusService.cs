using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Mail;
using Application.Models.Statuses;
using Domain.Statuses;
using Domain.Tickets;
using IMailService = Application.Mail.IMailService;
using Status = Domain.Statuses.Status;

namespace Application.Statuses
{
    public class StatusService : IStatusService
    {
        private readonly IStatusCollection _statusCollection;
        private readonly ITicketCollection _ticketCollection;
        private readonly IMailService _mailService;

        public StatusService(IStatusCollection statusCollection, ITicketCollection ticketCollection, IMailService mailService)
        {
            _statusCollection = statusCollection;
            _ticketCollection = ticketCollection;
            _mailService = mailService;
        }
        public async Task<GetStatusModel> CreateStatus(AddStatusModel model, CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                throw new Exception("Status Empty");
            }

            var status = new Status()
            {
                State = model.State,
                TicketId = model.TicketId
            };

            var search = await _statusCollection.CreateTicketStatus(status, cancellationToken);

            var result = new GetStatusModel()
            {
                Id = search.Id,
                State = search.State,
                TicketId = search.TicketId
            };

            _mailService.SendEmail(new MailData()
            {
                EmailBody = $"Ticket number:{result.Id} is {result.State}",
                EmailSubject = $"Your Ticket Status",
            });
            
            return result;
        }

        public async Task<GetStatusModel> GetStatusById(string statusId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(statusId))
            {
                throw new Exception("Status not found");
            }

            var search = await _statusCollection.GetTicketStatus(statusId, cancellationToken);
            
            if (search == null)
            {
                return new GetStatusModel();
            }
            
            var result = new GetStatusModel()
            {
                Id = search.Id,
                State = search.State,
                TicketId = search.TicketId
            };

            return result;
        }

        public async Task<List<GetStatusModel>> GetAllStatuses(CancellationToken cancellationToken = default)
        {
            var searchResults = await _statusCollection.GetAllTicketStatuses(cancellationToken);

            if (searchResults == null || searchResults.Count < 1)
            {
                return new List<GetStatusModel>();
            }

            var result = new List<GetStatusModel>();
            
            foreach(var searchResult in searchResults)
            {
                var model = new GetStatusModel()
                {
                    Id = searchResult.Id,
                    State = searchResult.State,
                    TicketId = searchResult.TicketId
                };
                
                result.Add(model);
            }

            return result;
        }
    }
}