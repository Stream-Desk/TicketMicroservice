using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Tickets;
using System.Threading.Tasks;

namespace TicketProcessingWorker
{
    public interface ITicketConnector
    {
        Task<Ticket> GetNextTicket();
        Task RemoveTicket(Ticket ticket);
    }
}
