namespace TicketingSystem.Api.Features.Tickets.GetTicketList
{
    public class TicketSummaryResponse
    {
        public Guid Id { get; set; }
        public string AvatarUrl { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string Username { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = default!;
    }

}
