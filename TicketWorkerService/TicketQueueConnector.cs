using Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;

namespace TicketWorkerService
{
    public class TicketQueueConnector : ITicketConnector
    {
        private readonly ILogger<TicketQueueConnector> logger;
        private readonly QueueClient ticketQueueClient;

        public TicketQueueConnector(ILogger<TicketQueueConnector> logger)
        {
            this.logger = logger;

            var connectionString = Environment.GetEnvironmentVariable("STORAGE_CONNECTION");
            var queueOpts = new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 };

            ticketQueueClient = new QueueClient(connectionString, "user-tickets", queueOpts);
            ticketQueueClient.CreateIfNotExists();
        }
        public async Task<Ticket> GetNextTicket()
        {
            var response = await ticketQueueClient.ReceiveMessageAsync();
            if (response.Value != null)
            {
                var ticket = response.Value.Body.ToObjectFromJson<Ticket>();
                ticket.QueueMessageId = response.Value.MessageId;
                ticket.QueuePopReceipt = response.Value.PopReceipt;

                return ticket;
            }
            return null;
        }

        public async Task RemoveTicket(Ticket ticket)
        {
            await ticketQueueClient.DeleteMessageAsync(ticket.QueueMessageId, ticket.QueuePopReceipt);
        }
    }
}
