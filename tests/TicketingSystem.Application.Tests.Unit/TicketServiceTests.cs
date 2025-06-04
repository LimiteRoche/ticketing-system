
using FluentAssertions;
using Moq;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.Services;
using TicketingSystem.Domain.Common;
using TicketingSystem.Domain.Entities;
using TicketingSystem.SharedKernel.Results;


namespace TicketingSystem.Application.Tests.Unit
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly TicketService _sut;

        public TicketServiceTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _sut = new TicketService(_ticketRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUnresolvedTicketsAsync_ShouldReturnTickets()
        {
            // Arrange
            var tickets = new List<Ticket> { new Ticket("user1", "User 1", "subject", "desc", "avatarUrl") };
            var expectedResult = Result<IEnumerable<Ticket>>.Success(tickets);
            _ticketRepositoryMock
                .Setup(x => x.GetUnresolvedTicketsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.GetUnresolvedTicketsAsync(CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(tickets);
        }

        [Fact]
        public async Task GetTicketByIdAsync_ShouldReturnTicket_WhenFound()
        {
            var ticket = new Ticket("user1", "User 1", "subject", "desc", "avatarUrl");
            var expectedResult = Result<Ticket>.Success(ticket);

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _sut.GetTicketByIdAsync(ticket.Id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(ticket);
        }

        [Fact]
        public async Task GetTicketByIdAsync_ShouldReturnFailure_WhenNotFound()
        {
            var errorMsg = "Ticket not found";
            var expectedResult = Result<Ticket>.Failure(errorMsg);

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _sut.GetTicketByIdAsync(Guid.NewGuid(), CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(errorMsg);
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldReturnSuccess_WhenCreated()
        {
            var userId = "user1";
            var username = "User 1";
            var subject = "subject";
            var description = "description";
            var avatarUrl = "avatarUrl";

            var ticket = new Ticket(userId, username, subject, description, avatarUrl);
            var expectedResult = Result<Ticket>.Success(ticket);

            _ticketRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Ticket>(t => t.Subject == subject && t.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _sut.CreateTicketAsync(userId, username, subject, description, avatarUrl, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Subject.Should().Be(subject);
            result.Value.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldReturnFailure_WhenDomainExceptionThrown()
        {
            var userId = "user1";
            var username = "User 1";
            var subject = "subject";
            var description = "description";
            var avatarUrl = "avatarUrl";

            _ticketRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DomainException("Domain error"));

            var result = await _sut.CreateTicketAsync(userId, username, subject, description, avatarUrl, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Domain error");
        }

        [Fact]
        public async Task CreateTicketAsync_ShouldReturnFailure_WhenUnexpectedExceptionThrown()
        {
            _ticketRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _sut.CreateTicketAsync("user", "name", "subj", "desc", "avatar", CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Unexpected error");
        }

        [Fact]
        public async Task AddReplyAsync_ShouldReturnSuccess_WhenReplyAdded()
        {
            var ticketId = Guid.NewGuid();
            var message = "Reply message";
            var agentName = "Agent Smith";
            var avatarUrl = "avatarUrl";

            var ticket = new Ticket("user1", "User 1", "subject", "desc", "avatarUrl");
            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Success(ticket));

            _ticketRepositoryMock
                .Setup(x => x.UpdateAsync(ticket, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await _sut.AddReplyAsync(ticketId, message, agentName, avatarUrl, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _ticketRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Ticket>(t => t.Replies.Count > 0), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddReplyAsync_ShouldReturnFailure_WhenTicketNotFound()
        {
            var ticketId = Guid.NewGuid();
            var errorMsg = "Ticket not found";

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Failure(errorMsg));

            var result = await _sut.AddReplyAsync(ticketId, "msg", "agent", "avatar", CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(errorMsg);
        }

        [Fact]
        public async Task AddReplyAsync_ShouldReturnFailure_WhenDomainExceptionThrown()
        {
            var ticketId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Success(new Ticket("user1", "User1", "subj", "desc", "avatar")));

            _ticketRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DomainException("Domain error"));

            var result = await _sut.AddReplyAsync(ticketId, "msg", "agent", "avatar", CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Domain error");
        }

        [Fact]
        public async Task AddReplyAsync_ShouldReturnFailure_WhenUnexpectedExceptionThrown()
        {
            var ticketId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Success(new Ticket("user1", "User1", "subj", "desc", "avatar")));

            _ticketRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _sut.AddReplyAsync(ticketId, "msg", "agent", "avatar", CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Unexpected error");
        }

        [Fact]
        public async Task MarkAsResolvedAsync_ShouldReturnSuccess_WhenMarked()
        {
            var ticketId = Guid.NewGuid();

            var ticket = new Ticket("user1", "User1", "subj", "desc", "avatar");
            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Success(ticket));

            _ticketRepositoryMock
                .Setup(x => x.UpdateAsync(ticket, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            var result = await _sut.MarkAsResolvedAsync(ticketId, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task MarkAsResolvedAsync_ShouldReturnFailure_WhenTicketNotFound()
        {
            var ticketId = Guid.NewGuid();
            var errorMsg = "Ticket not found";

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Failure(errorMsg));

            var result = await _sut.MarkAsResolvedAsync(ticketId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(errorMsg);
        }

        [Fact]
        public async Task MarkAsResolvedAsync_ShouldReturnFailure_WhenUpdateFails()
        {
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket("user1", "User1", "subj", "desc", "avatar");

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Success(ticket));

            var updateError = "Failed to update";
            _ticketRepositoryMock
                .Setup(x => x.UpdateAsync(ticket, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure(updateError));

            var result = await _sut.MarkAsResolvedAsync(ticketId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(updateError);
        }

        [Fact]
        public async Task MarkAsResolvedAsync_ShouldReturnFailure_WhenDomainExceptionThrown()
        {
            var ticketId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Success(new Ticket("user1", "User1", "subj", "desc", "avatar")));

            _ticketRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DomainException("Domain error"));

            var result = await _sut.MarkAsResolvedAsync(ticketId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Domain error");
        }

        [Fact]
        public async Task MarkAsResolvedAsync_ShouldReturnFailure_WhenUnexpectedExceptionThrown()
        {
            var ticketId = Guid.NewGuid();

            _ticketRepositoryMock
                .Setup(x => x.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Ticket>.Success(new Ticket("user1", "User1", "subj", "desc", "avatar")));

            _ticketRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _sut.MarkAsResolvedAsync(ticketId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("Unexpected error");
        }

    }



}
