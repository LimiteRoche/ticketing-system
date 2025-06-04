namespace TicketingSystem.Api.Features.Tickets.AddReply
{
    public class AddReplyRequest
    {
        public string Message { get; set; } = default!;
        public string AgentName { get; set; } = default!;
        public string AvatarUrl { get; set; } = default!;
    }

}
