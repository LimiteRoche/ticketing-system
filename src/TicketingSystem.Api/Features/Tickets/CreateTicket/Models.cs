namespace TicketingSystem.Api.Features.Tickets.CreateTicket
{
    public class CreateTicketRequest
    {
        public string UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string AvatarUrl { get; set; } = default!;
    }

    public class CreateTicketResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = default!;

        public string Description { get; set; } = default!;
        public string AvatarUrl { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = default!;
    }

}
