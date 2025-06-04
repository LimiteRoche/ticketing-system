using TicketingSystem.Domain.Common;

namespace TicketingSystem.Domain.Exceptions;

public class TicketAlreadyResolvedException : DomainException
{
    public TicketAlreadyResolvedException(Guid ticketId)
        : base($"Ticket {ticketId} is already resolved.") { }
}
