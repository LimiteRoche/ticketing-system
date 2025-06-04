using FastEndpoints;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Api.Features.Tickets.GetTicketList
{
    public class TicketSummaryMapper : ResponseMapper<TicketSummaryResponse, Ticket>
    {
        public override TicketSummaryResponse FromEntity(Ticket t) => new()
        {
            Id = t.Id,
            Subject = t.Subject,
            AvatarUrl = t.AvatarUrl,
            Username = t.Username,
            CreatedAt = t.CreatedAt,
            Status = t.Status.ToString(),
        };
    }
}
