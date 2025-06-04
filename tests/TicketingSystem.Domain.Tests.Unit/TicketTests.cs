using FluentAssertions;
using TicketingSystem.Domain.Common;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Enums;
using TicketingSystem.Domain.Exceptions;


namespace TicketingSystem.Domain.Tests.Entities;

public class TicketTests
{
    [Fact]
    public void Constructor_ShouldCreateValidTicket()
    {
        var ticket = new Ticket("user1", "Alice", "Subject", "Description", "avatar.png");

        ticket.UserId.Should().Be("user1");
        ticket.Username.Should().Be("Alice");
        ticket.Subject.Should().Be("Subject");
        ticket.Description.Should().Be("Description");
        ticket.AvatarUrl.Should().Be("avatar.png");
        ticket.Status.Should().Be(TicketStatus.Open);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenSubjectInvalid(string invalidSubject)
    {
        Action act = () => new Ticket("user1", "Alice", invalidSubject, "desc", "avatar");

        act.Should().Throw<DomainException>().WithMessage("*Subject is required*");
    }

    [Fact]
    public void AddReply_ShouldAddReply_AndChangeStatusToInResolution()
    {
        var ticket = new Ticket("user1", "Alice", "Sub", "Desc", "avatar");
        ticket.Status.Should().Be(TicketStatus.Open);

        ticket.AddReply("Message", "Agent", "avatar2");

        ticket.Replies.Should().HaveCount(1);
        ticket.Status.Should().Be(TicketStatus.InResolution);
    }

    [Fact]
    public void AddReply_ShouldThrow_WhenTicketIsResolved()
    {
        var ticket = new Ticket("user1", "Alice", "Sub", "Desc", "avatar");
        ticket.MarkAsResolved();

        Action act = () => ticket.AddReply("Hi", "Agent", "avatar");

        act.Should().Throw<DomainException>().WithMessage("*Cannot add reply*");
    }

    [Fact]
    public void MarkAsResolved_ShouldSetStatusToResolved()
    {
        var ticket = new Ticket("user1", "Alice", "Sub", "Desc", "avatar");

        ticket.MarkAsResolved();

        ticket.Status.Should().Be(TicketStatus.Resolved);
    }

    [Fact]
    public void MarkAsResolved_ShouldThrow_WhenAlreadyResolved()
    {
        var ticket = new Ticket("user1", "Alice", "Sub", "Desc", "avatar");
        ticket.MarkAsResolved();

        Action act = () => ticket.MarkAsResolved();

        act.Should().Throw<TicketAlreadyResolvedException>();
    }

    [Fact]
    public void UpdateDetails_ShouldChangeSubjectAndDescription()
    {
        var ticket = new Ticket("user1", "Alice", "Sub", "Desc", "avatar");

        ticket.UpdateDetails("New Subject", "New Description");

        ticket.Subject.Should().Be("New Subject");
        ticket.Description.Should().Be("New Description");
    }

    [Fact]
    public void UpdateDetails_ShouldThrow_WhenTicketIsResolved()
    {
        var ticket = new Ticket("user1", "Alice", "Sub", "Desc", "avatar");
        ticket.MarkAsResolved();

        Action act = () => ticket.UpdateDetails("X", "Y");

        act.Should().Throw<DomainException>().WithMessage("*Cannot update*");
    }
}
