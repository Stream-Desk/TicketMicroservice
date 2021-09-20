using System;
using Coravel.Invocable;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Domain.Tickets;
using MessageSender.Services;

namespace TicketWorkerService 
{
    public class ProcessTicket : IInvocable
    {
        private readonly ILogger<ProcessTicket> logger;
        private readonly IQueueService ticketConnector;
   
        public ProcessTicket(ILogger<ProcessTicket> logger)
        {
            this.logger = logger;
            this.ticketConnector = ticketConnector;
        }
        public async Task Invoke()
        {
            var nextTicket = await QueueService.GettNextTicket();
            if (nextTicket != null)
            {
                logger.LogInformation("Processing ticket {@nextTicket}", nextTicket);

                //Todo: Implement ticket processing

                await ticketConnector.RemoveTicket(nextTicket);
            }
           
       
        }
    }
}
