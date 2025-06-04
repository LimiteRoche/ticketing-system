using System;
using FluentAssertions;
using TicketingSystem.Domain.Common;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Domain.Exceptions;
using Xunit;

namespace TicketingSystem.Domain.Tests.Entities;

public class ReplyTests
{
    [Fact]
    public void Constructor_ShouldCreateValidReply()
    {
        var ticketId = Guid.NewGuid();

        var reply = new Reply(ticketId, "Hello", "Agent Smith", "avatar.jpg");

        reply.TicketId.Should().Be(ticketId);
        reply.Message.Should().Be("Hello");
        reply.AgentName.Should().Be("Agent Smith");
        reply.AvatarUrl.Should().Be("avatar.jpg");
        reply.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenMessageIsEmpty()
    {
        Action act = () => new Reply(Guid.NewGuid(), "", "Agent", "avatar");

        act.Should().Throw<DomainException>().WithMessage("*Reply message is required*");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenAgentNameIsEmpty()
    {
        Action act = () => new Reply(Guid.NewGuid(), "Hello", "", "avatar");

        act.Should().Throw<DomainException>().WithMessage("*Agent name is required*");
    }
}
