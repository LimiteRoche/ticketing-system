namespace TicketingSystem.Infrastructure.Persistence.Entities
{
    public class TicketEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ReplyEntity> Replies { get; set; } = new List<ReplyEntity>();
    }
}
