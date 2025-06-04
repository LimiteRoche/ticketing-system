using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.SharedKernel.Results;
using TicketingSystem.Domain.Common;

namespace TicketingSystem.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Result<IEnumerable<Ticket>>> GetUnresolvedTicketsAsync(CancellationToken ct)
            => await _ticketRepository.GetUnresolvedTicketsAsync(ct);

        public async Task<Result<Ticket>> GetTicketByIdAsync(Guid ticketId, CancellationToken ct)
            => await _ticketRepository.GetByIdAsync(ticketId, ct);

        public async Task<Result<Ticket>> CreateTicketAsync(string userId, string username, string subject, string description, string avatarUrl, CancellationToken ct)
        {
            try
            {
                var ticket = new Ticket(userId, username, subject, description, avatarUrl);
                return await _ticketRepository.AddAsync(ticket, ct);
            }
            catch (DomainException ex)
            {
                return Result<Ticket>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<Ticket>.Failure($"Unexpected error while creating ticket: {ex.Message}");
            }
        }

        public async Task<Result> AddReplyAsync(Guid ticketId, string message, string agentName, string avatarUrl, CancellationToken ct)
        {
            try
            {
                var ticketResult = await _ticketRepository.GetByIdAsync(ticketId, ct);
                if (ticketResult.IsFailure)
                    return Result.Failure(ticketResult.Error!);

                ticketResult.Value!.AddReply(message, agentName, avatarUrl);
                return await _ticketRepository.UpdateAsync(ticketResult.Value, ct);
            }
            catch (DomainException ex)
            {
                return Result<Ticket>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<Ticket>.Failure($"Unexpected error while creating ticket: {ex.Message}");
            }

        }


        public async Task<Result> MarkAsResolvedAsync(Guid ticketId, CancellationToken ct)
        {
            try
            {
                var ticketResult = await _ticketRepository.GetByIdAsync(ticketId, ct);
                if (ticketResult.IsFailure)
                    return Result.Failure(ticketResult.Error!);

                ticketResult.Value.MarkAsResolved();

                var updateResult = await _ticketRepository.UpdateAsync(ticketResult.Value, ct);
                if (updateResult.IsFailure)
                    return Result.Failure(updateResult.Error!);
            }
            catch (DomainException ex)
            {
                return Result<Ticket>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<Ticket>.Failure($"Unexpected error while creating ticket: {ex.Message}");
            }

            return Result.Success();
        }
    }
}
