using System;
using Coravel.Invocable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Domain.Tickets;

namespace TicketWorkerService 
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
                Summary ="cant send pictures",
                Description = "i am failing to send emails",
                TicketNumber = "6"
            };

            logger.LogInformation("Processing ticket{@ticket}", ticket);

            return Task.FromResult(true);
        }
    }
}
