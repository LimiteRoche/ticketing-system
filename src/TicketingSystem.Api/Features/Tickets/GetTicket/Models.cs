namespace TicketingSystem.Api.Features.Tickets.GetTicket
{
    public class GetTicketRequest
    {
        public Guid TicketId { get; set; }
    }

    public class GetTicketResponse
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string AvatarUrl { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public List<ReplyDto> Replies { get; set; } = [];
    }

    public class ReplyDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = default!;
        public string AgentName { get; set; } = default!;
        public string AvatarUrl { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
    }
}
