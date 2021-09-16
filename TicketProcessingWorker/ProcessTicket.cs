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
        private readonly ITicketConnector ticketConnector;

        public ProcessTicket(ILogger<ProcessTicket> logger, ITicketConnector ticketConnector)
        {
            this.logger = logger;
            this.ticketConnector = ticketConnector;
        }
        public Task Invoke()

        {

            var ticket = new Ticket

            {

                Summary = "testing",
                Description = "testing testing"

            };

            logger.LogInformation("Processing ticket {@ticket}", ticket);

            return Task.FromResult(true);

        }
    }
}
