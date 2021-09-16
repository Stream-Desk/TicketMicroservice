using Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketWorkerService
{
    public interface ITicketConnector
    {
        Task<Ticket> GetNextTicket();

        Task RemoveTicket(Ticket ticket);
    }
}
