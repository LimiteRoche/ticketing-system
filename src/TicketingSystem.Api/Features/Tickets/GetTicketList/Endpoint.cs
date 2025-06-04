using FastEndpoints;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Api.Features.Tickets.GetTicketList
{
    public class GetTicketListEndpoint : EndpointWithoutRequest<List<TicketSummaryResponse>, TicketSummaryMapper>
    {
        private readonly ITicketService _ticketService;

        public GetTicketListEndpoint(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public override void Configure()
        {
            Get("/tickets");
            Summary(s =>
            {
                s.Summary = "List unresolved tickets";
                s.Description = "Returns all tickets that have not yet been marked as resolved.";
            });
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var result = await _ticketService.GetUnresolvedTicketsAsync(ct);

            if (result.IsFailure)
            {
                AddError(result.Error!);
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            var mapped = result.Value!.Select(Map.FromEntity).ToList();
            await SendAsync(mapped, cancellation: ct);
        }
    }
}
