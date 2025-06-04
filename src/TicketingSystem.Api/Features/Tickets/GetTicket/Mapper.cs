using FastEndpoints;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Api.Features.Tickets.GetTicket
{
    public class GetTicketMapper : ResponseMapper<GetTicketResponse, Ticket>
    {
        public override GetTicketResponse FromEntity(Ticket t) => new()
        {
            Id = t.Id,
            UserId = t.UserId,
            Username = t.Username,
            Subject = t.Subject,
            Description = t.Description,
            AvatarUrl = t.AvatarUrl,
            Status = t.Status.ToString(),
            CreatedAt = t.CreatedAt,
            Replies = t.Replies.Select(r => new ReplyDto
            {
                Id = r.Id,
                Message = r.Message,
                AgentName = r.AgentName,
                AvatarUrl = r.AvatarUrl,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }
}