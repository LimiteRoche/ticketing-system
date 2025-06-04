using FastEndpoints;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Api.Features.Tickets.AddReply
{
    public class AddReplyEndpoint : Endpoint<AddReplyRequest>
    {
        private readonly ITicketService _ticketService;

        public AddReplyEndpoint(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public override void Configure()
        {
            Post("/tickets/{ticketId:guid}/replies");
            Summary(s =>
            {
                s.Summary = "Add a reply to an existing ticket";
                s.Description = "Allows a support agent to reply to a specific ticket.";
            });
            AllowAnonymous();
        }

        public override async Task HandleAsync(AddReplyRequest req, CancellationToken ct)
        {
            var ticketId = Route<Guid>("ticketId");

            var result = await _ticketService.AddReplyAsync(ticketId, req.Message, req.AgentName, req.AvatarUrl, ct);

            if (result.IsFailure)
            {
                AddError(result.Error!);
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            await SendNoContentAsync(ct);
        }
    }
}
