using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Enums;
using TicketingSystem.Infrastructure.Persistence.Entities;

public static class TicketMapper
{
    public static TicketEntity ToEntity(Ticket ticket)
    {
        return new TicketEntity
        {
            Id = ticket.Id,
            UserId = ticket.UserId,
            Username = ticket.Username,
            Subject = ticket.Subject,
            Description = ticket.Description,
            AvatarUrl = ticket.AvatarUrl,
            Status = (int)ticket.Status,
            CreatedAt = ticket.CreatedAt,
            Replies = ticket.Replies.Select(ToEntity).ToList()
        };
    }

    public static ReplyEntity ToEntity(Reply reply)
    {
        return new ReplyEntity
        {
            Id = reply.Id,
            TicketId = reply.TicketId,
            Message = reply.Message,
            AvatarUrl = reply.AvatarUrl,
            AgentName = reply.AgentName,
            CreatedAt = reply.CreatedAt
        };
    }

    public static Ticket ToDomain(TicketEntity entity)
    {
        var replies = entity.Replies?
            .Select(ToDomain)
            .Where(r => r is not null)
            .ToList() ?? new List<Reply>();

        return new Ticket(
            entity.Id,
            entity.UserId,
            entity.Username,
            entity.Subject,
            entity.Description,
            entity.AvatarUrl,
            (TicketStatus)entity.Status,
            replies,
            entity.CreatedAt
        );
    }

    public static Reply ToDomain(ReplyEntity entity)
    {
        return new Reply(
            entity.Id,
            entity.TicketId,
            entity.Message,
            entity.AgentName,
            entity.AvatarUrl,
            entity.CreatedAt
        );
    }

    public static void UpdateEntity(TicketEntity entity, Ticket domain)
    {
        entity.Subject = domain.Subject;
        entity.Description = domain.Description;
        entity.Status = (int)domain.Status;
        entity.AvatarUrl = domain.AvatarUrl;

        var existingReplyIds = entity.Replies.Select(r => r.Id).ToHashSet();
        foreach (var reply in domain.Replies.Where(r => !existingReplyIds.Contains(r.Id)))
        {
            entity.Replies.Add(new ReplyEntity
            {
                TicketId = entity.Id,
                Message = reply.Message,
                AvatarUrl = reply.AvatarUrl,
                CreatedAt = reply.CreatedAt,
                AgentName = reply.AgentName
            });
        }
    }
}
