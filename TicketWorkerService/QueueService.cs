 using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TicketWorkerService;

namespace MessageSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;
        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public void SendMessage(string queueName, string message)
        {
            string connectionString = _configuration["StorageConnectionString"];
            var queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
            queueClient.CreateIfNotExists();
            if (queueClient.Exists())
            {
                queueClient.SendMessage(message);
            }
        }

        internal static Task GettNextTicket()
        {
            throw new NotImplementedException();
        }
    }
}