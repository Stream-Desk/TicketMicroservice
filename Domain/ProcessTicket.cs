using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coravel.Invocable;
using Domain.Tickets;
using Microsoft.Extensions.Logging;


namespace TicketProcessingWorker
{
    public class ProcessTicket : IInvocable
    {
        private readonly ILogger<ProcessTicket> logger;

        public ProcessTicket(ILogger<ProcessTicket> logger)
        {
            this.logger = logger;
        }
        public Task Invoke()

        {
            var ticket = new Ticket

            {
               
                Summary = "testing",
                Description = "testing testing"

            };
            var jobId = Guid.NewGuid();
            logger.LogInformation("Processing ticket {@ticket}",ticket);

            return Task.FromResult(true);
        }
    }
}
