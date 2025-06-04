using FastEndpoints;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Api.Features.Tickets.CreateTicket
{
    public class CreateTicketMapper : Mapper<CreateTicketRequest, CreateTicketResponse, Ticket>
    {
        public override CreateTicketResponse FromEntity(Ticket t)
        {
            return new CreateTicketResponse
            {
                Id = t.Id,
                Subject = t.Subject,
                Description = t.Description,
                AvatarUrl = t.AvatarUrl,
                Username = t.Username,
                UserId = t.UserId,
                CreatedAt = t.CreatedAt,
                Status = t.Status.ToString()
            };
        }
    }
}