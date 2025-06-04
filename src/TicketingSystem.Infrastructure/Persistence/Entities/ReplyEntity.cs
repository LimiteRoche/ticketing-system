namespace TicketingSystem.Infrastructure.Persistence.Entities
{
    public class ReplyEntity
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public string Message { get; set; } = null!;
        public string AgentName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public TicketEntity Ticket { get; set; }
    }
}
