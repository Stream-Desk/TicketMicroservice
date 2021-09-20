using Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketWorkerService
{
    public interface IQueueService
    {
        public interface IQueueService
        {
            void SendMessage(string queueName, string message);
        }

        Task GetNextTicket();
    }
}
