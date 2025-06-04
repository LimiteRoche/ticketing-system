using TicketingSystem.Domain.Entities;
using TicketingSystem.SharedKernel.Results;

namespace TicketingSystem.Application.Interfaces
{
    public interface ITicketService
    {
        Task<Result<Ticket>> CreateTicketAsync(string userId, string username, string subject, string description, string avatarUrl, CancellationToken ct);
        Task<Result<IEnumerable<Ticket>>> GetUnresolvedTicketsAsync(CancellationToken ct);
        Task<Result<Ticket>> GetTicketByIdAsync(Guid ticketId, CancellationToken ct);
        Task<Result> AddReplyAsync(Guid ticketId, string message, string agentName, string avatarUrl, CancellationToken ct);
        Task<Result> MarkAsResolvedAsync(Guid ticketId, CancellationToken ct);
    }
}
