using FastEndpoints;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Api.Features.Tickets.GetTicket
{
    public class GetTicketEndpoint : Endpoint<GetTicketRequest, GetTicketResponse, GetTicketMapper>
    {
        private readonly ITicketService _service;

        public GetTicketEndpoint(ITicketService service)
        {
            _service = service;
        }

        public override void Configure()
        {
            Get("/tickets/{ticketId}");
            Summary(s =>
            {
                s.Summary = "Get a specific ticket by ID.";
                s.Description = "Returns ticket details including replies.";
                s.Responses[200] = "Ticket found";
                s.Responses[404] = "Ticket not found";
            });
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetTicketRequest req, CancellationToken ct)
        {
            var result = await _service.GetTicketByIdAsync(req.TicketId, ct);

            if (result.IsFailure)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendOkAsync(Map.FromEntity(result.Value!), ct);
        }
    }
}

