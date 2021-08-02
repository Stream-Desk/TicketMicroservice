using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Tickets
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetTickets();
        Task<Ticket> GetTicketById(Guid ticketId);
        Task<Ticket> CreateTicket(Ticket ticket);
        void UpdateTicket(Guid ticketId, Ticket ticket);
        void DeleteTicket(Guid ticketId);
        void DeleteTicketById(Guid ticketId);

    }
}