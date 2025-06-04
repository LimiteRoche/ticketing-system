using Bogus;
using TicketingSystem.Api.Features.Tickets.GetTicketList;
using TicketingSystem.Api.Features.Tickets.CreateTicket;
using TicketingSystem.Api.Features.Tickets.GetTicket;
using FastEndpoints;
using TicketingSystem.Api.Features.Tickets.AddReply;
using TicketingSystem.Api.Features.Tickets.ResolveTicket;
using TicketingSystem.Domain.Entities;
using System.Security.Cryptography;

namespace TicketingSystem.Api.Tests.Integration
{

    public class TicketTests(App App) : TestBase<App>
    {
        [Fact]
        public async Task Full_Ticket_Lifecycle_Flow()
        {
            var faker = new Faker();

            // 1. Create 2 tickets
            var tickets = new List<(Guid Id, string UserId, string Username, string Subject, string Description)>();

            for (int i = 0; i < 2; i++)
            {
                var req = new CreateTicketRequest
                {
                    Subject = faker.Lorem.Sentence(5),
                    Description = faker.Lorem.Paragraph(),
                    AvatarUrl = faker.Internet.Avatar(),
                    UserId = faker.Random.Guid().ToString(),
                    Username = faker.Internet.UserName()
                };

                var (rsp, res) = await App.Client.POSTAsync<CreateTicketEndpoint,
                    CreateTicketRequest,
                    CreateTicketResponse>(req);

                rsp.IsSuccessStatusCode.Should().BeTrue();
                res.Id.Should().NotBeEmpty();

                tickets.Add((res.Id, req.UserId, req.Username, req.Subject, req.Description));
            }

            var secondTicketId = tickets[1].Id;

            // 2. Add Comment to second ticket
            var replyReq = new AddReplyRequest
            {
                AgentName = faker.Name.FullName(),
                AvatarUrl = faker.Internet.Avatar(),
                Message = "Not it should work :D."
            };

            var(rspReplies, _) = await App.Client.POSTAsync<AddReplyRequest, object>($"api/tickets/{secondTicketId}/replies", replyReq);
            rspReplies.IsSuccessStatusCode.Should().BeTrue();
  

            // 3. Get all tickets
            var (getAllRsp, allTickets) = await App.Client.GETAsync<GetTicketListEndpoint,
                List<TicketSummaryResponse>>();

            getAllRsp.IsSuccessStatusCode.Should().BeTrue();
            allTickets.Should().Contain(x => x.Id == tickets[0].Id);
            allTickets.Should().Contain(x => x.Id == tickets[1].Id);

            // 4. Get Ticket details for both tickets
            foreach (var (id, userId, username, subject, description) in tickets)
            {
                var (getRsp, ticket) = await App.Client.GETAsync<GetTicketEndpoint,
                    GetTicketRequest,
                    GetTicketResponse>(new GetTicketRequest { TicketId = id });

                getRsp.IsSuccessStatusCode.Should().BeTrue();
                ticket.Id.Should().Be(id);
                ticket.UserId.Should().Be(userId);
                ticket.Username.Should().Be(username);
                ticket.Subject.Should().Be(subject);
                ticket.Description.Should().Be(description);

                if (id == secondTicketId)
                {
                    ticket.Status.Should().Be("InResolution");
                    ticket.Replies.Should().ContainSingle();
                    ticket.Replies[0].Message.Should().Be(replyReq.Message);
                    ticket.Replies[0].AgentName.Should().Be(replyReq.AgentName);
                }
                else
                {
                    ticket.Status.Should().Be("Open");
                    ticket.Replies.Should().BeEmpty();
                }
            }

            // 5. Mark second ticket as closed
            var(rspResolve, _) = await App.Client.PATCHAsync<object,object>(
                   $"api/tickets/{secondTicketId}/resolve", new { } 
               );
            rspResolve.IsSuccessStatusCode.Should().BeTrue();


            // 6. Verify that the second ticket is now closed (not display in the list)
            var (finalListRsp, finalTickets) = await App.Client.GETAsync<GetTicketListEndpoint,
                List<TicketSummaryResponse>>();

            finalListRsp.IsSuccessStatusCode.Should().BeTrue();
            finalTickets.Should().Contain(x => x.Id == tickets[0].Id);
            finalTickets.Should().NotContain(x => x.Id == secondTicketId);
        }

    }
}
