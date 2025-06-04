using FastEndpoints;
using TicketingSystem.Api.Features.Tickets.GetTicket;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Api.Features.Tickets.CreateTicket
{
    public class CreateTicketEndpoint : Endpoint<CreateTicketRequest, CreateTicketResponse, CreateTicketMapper>
    {
        private readonly ITicketService _ticketService;

        public CreateTicketEndpoint(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public override void Configure()
        {
            Post("/tickets");
            Summary(s =>
            {
                s.Summary = "Creates a new support ticket.";
                s.Description = "Creates a new support ticket with subject and description.";
            });
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateTicketRequest req, CancellationToken ct)
        {
            var result = await _ticketService.CreateTicketAsync(req.UserId, req.Username, req.Subject, req.Description, req.AvatarUrl, ct);

            if (result.IsFailure)
            {
                AddError(message: result.Error!);
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            await SendCreatedAtAsync<GetTicketEndpoint>(new { id = result.Value!.Id }, Map.FromEntity(result.Value), cancellation: ct);
        }
    }

}
