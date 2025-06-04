using TicketingSystem.Domain.Common;
using TicketingSystem.SharedKernel.Results;

namespace TicketingSystem.Domain.Entities
{
    public class Reply
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid TicketId { get; private set; }
        public string Message { get; private set; }
        public string AgentName { get; private set; }
        public string AvatarUrl { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        internal Reply(Guid id, Guid ticketId, string message, string agentName, string avatarUrl, DateTime createdAt)
        {
            Id = id;
            TicketId = ticketId;
            Message = message;
            AgentName = agentName;
            AvatarUrl = avatarUrl;
            CreatedAt = createdAt;
        }

        public Reply(Guid ticketId, string message, string agentName, string avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new DomainException("Reply message is required.");
            if (string.IsNullOrWhiteSpace(agentName))
                throw new DomainException("Agent name is required.");

            TicketId = ticketId;
            Message = message;
            AgentName = agentName;
            AvatarUrl = avatarUrl;
        }
    }
}
