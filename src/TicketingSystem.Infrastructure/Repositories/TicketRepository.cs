using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Domain.Enums;
using TicketingSystem.SharedKernel.Results;
using TicketingSystem.Infrastructure.Persistence.Entities;

namespace TicketingSystem.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketingDbContext _context;

        public TicketRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Ticket>> AddAsync(Ticket ticket, CancellationToken ct)
        {
            try
            {
                var ticketEntity = TicketMapper.ToEntity(ticket);
                await _context.Tickets.AddAsync(ticketEntity, ct);
                await _context.SaveChangesAsync(ct);

                var savedTicket = await _context.Tickets
                    .AsNoTracking()
                    .Include(t => t.Replies)
                    .FirstOrDefaultAsync(t => t.Id == ticketEntity.Id);

    
                var domainTicket = TicketMapper.ToDomain(savedTicket);

                return Result<Ticket>.Success(domainTicket);

            }
            catch (DbUpdateException ex)
            {
                return Result<Ticket>.Failure($"Database error while adding ticket: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<Ticket>.Failure($"Unexpected error while adding ticket: {ex.Message}");
            }
        }


        public async Task<Result> UpdateAsync(Ticket ticket, CancellationToken ct)
        {
            try
            {
                var existingEntity = await _context.Tickets
                    .Include(t => t.Replies)
                    .FirstOrDefaultAsync(t => t.Id == ticket.Id, ct);

                if (existingEntity is null)
                    return Result.Failure($"Ticket with ID {ticket.Id} not found in database");


                TicketMapper.UpdateEntity(existingEntity, ticket);

                await _context.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result.Failure($"Concurrency error while updating ticket: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure($"Database error while updating ticket: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Unexpected error while updating ticket: {ex.Message}");
            }
        }

        public async Task<Result> AddReplyAsync(Guid ticketId, Reply reply, CancellationToken ct)
        {
            try
            {
                var ticketEntity = await _context.Tickets
                    .Include(t => t.Replies)
                    .FirstOrDefaultAsync(t => t.Id == ticketId, ct);

                if (ticketEntity == null)
                    return Result.Failure($"Ticket with ID {ticketId} not found");

                var replyEntity = new ReplyEntity
                {
                    Id = reply.Id,
                    Message = reply.Message,
                    CreatedAt = reply.CreatedAt
                };

                ticketEntity.Replies.Add(replyEntity);

                await _context.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error adding reply to ticket {ticketId}: {ex.Message}");
            }
        }

        public async Task<Result<Ticket>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Tickets.AsNoTracking()
                .Include(t => t.Replies.OrderBy(r => r.CreatedAt)).FirstOrDefaultAsync(t => t.Id == id, ct);
            if (entity == null) 
                return Result<Ticket>.Failure("Ticket not found");

            return Result<Ticket>.Success(TicketMapper.ToDomain(entity));
        }

        public async Task<Result<IEnumerable<Ticket>>> GetUnresolvedTicketsAsync(CancellationToken ct)
        {
            var unresolved = await _context.Tickets
                .Include(t => t.Replies.OrderBy(r => r.CreatedAt))
                .Where(t => t.Status != (int)TicketStatus.Resolved)
                .ToListAsync(ct);

            return Result<IEnumerable<Ticket>>.Success(unresolved.Select(TicketMapper.ToDomain));
        }
    }
}