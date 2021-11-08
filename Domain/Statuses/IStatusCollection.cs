using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Statuses
{
    public interface IStatusService
    {
        Task<Status> CreateTicketStatus(CancellationToken cancellationToken = default);
        Task<Status> GetTicketStatus(string statusId, CancellationToken cancellationToken = default);
        Task<List<Status>> GetAllTicketStatuses(CancellationToken cancellationToken = default);
    }
}