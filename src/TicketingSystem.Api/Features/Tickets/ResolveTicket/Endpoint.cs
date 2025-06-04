using FastEndpoints;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Api.Features.Tickets.ResolveTicket
{

    public class ResolveTicketEndpoint : EndpointWithoutRequest
    {
        private readonly ITicketService _ticketService;

        public ResolveTicketEndpoint(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public override void Configure()
        {
            Patch("/tickets/{ticketId:guid}/resolve");
            Summary(s =>
            {
                s.Summary = "Mark a ticket as resolved";
                s.Description = "Sets the status of a ticket to resolved by its ID.";
            });
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var ticketId = Route<Guid>("ticketId");

            var result = await _ticketService.MarkAsResolvedAsync(ticketId, ct);

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