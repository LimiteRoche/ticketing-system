using TicketingSystem.Domain.Entities;
using TicketingSystem.SharedKernel.Results;

namespace TicketingSystem.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<Result<Ticket>> AddAsync(Ticket ticket, CancellationToken ct);
        Task<Result> UpdateAsync(Ticket ticket, CancellationToken ct);
        Task<Result> AddReplyAsync(Guid ticketId, Reply reply, CancellationToken ct);
        Task<Result<Ticket>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Result<IEnumerable<Ticket>>> GetUnresolvedTicketsAsync(CancellationToken ct);
    }
}