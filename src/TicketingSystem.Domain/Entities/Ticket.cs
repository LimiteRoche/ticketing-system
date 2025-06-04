using TicketingSystem.Domain.Enums;
using TicketingSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using TicketingSystem.Domain.Common;
using TicketingSystem.Domain.Exceptions;

namespace TicketingSystem.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string UserId { get; private set; }
        public string Username { get; private set; }
        public string Subject { get; private set; }
        public string Description { get; private set; }
        public string AvatarUrl { get; private set; }
        public TicketStatus Status { get; private set; } = TicketStatus.Open;
        private readonly List<Reply> _replies = new();
        public IReadOnlyList<Reply> Replies => _replies.AsReadOnly();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        internal Ticket(Guid id, string userId, string username, string subject,
                       string description, string avatarUrl, TicketStatus status,
                       List<Reply> replies, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Username = username;
            Subject = subject;
            Description = description;
            AvatarUrl = avatarUrl;
            Status = status;
            _replies = replies ?? new List<Reply>();
            CreatedAt = createdAt;
        }

        public Ticket(string userId, string username, string subject,
                     string description, string avatarUrl)
        {
            ValidateCreationParameters(userId, username, subject, description, avatarUrl);

            UserId = userId;
            Username = username;
            Subject = subject;
            Description = description;
            AvatarUrl = avatarUrl;
        }

        private static void ValidateCreationParameters(string userId, string username,
                                                      string subject, string description,
                                                      string avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new DomainException("UserId is required.");
            if (string.IsNullOrWhiteSpace(username))
                throw new DomainException("Username is required.");
            if (string.IsNullOrWhiteSpace(subject))
                throw new DomainException("Subject is required.");
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description is required.");
            if (string.IsNullOrWhiteSpace(avatarUrl))
                throw new DomainException("Avatar is required.");
        }

        public void AddReply(string message, string agentName, string avatarUrl)
        {
            if (Status == TicketStatus.Resolved)
                throw new DomainException("Cannot add reply to resolved ticket.");

            var reply = new Reply(Id, message, agentName, avatarUrl);
            _replies.Add(reply);

            if (Status == TicketStatus.Open)
                Status = TicketStatus.InResolution;
        }

        public void MarkAsResolved()
        {
            if (Status == TicketStatus.Resolved)
                throw new TicketAlreadyResolvedException(Id);

            Status = TicketStatus.Resolved;
        }

        public void UpdateDetails(string subject, string description)
        {
            if (Status == TicketStatus.Resolved)
                throw new DomainException("Cannot update details of a resolved ticket.");

            if (string.IsNullOrWhiteSpace(subject))
                throw new DomainException("Subject is required.");

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description is required.");

            Subject = subject;
            Description = description;
        }
    }
}