using Microsoft.EntityFrameworkCore;
using TicketingSystem.Infrastructure.Persistence.Configurations;
using TicketingSystem.Infrastructure.Persistence.Entities;

namespace TicketingSystem.Infrastructure.Persistence
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options)
            : base(options)
        { }

        public DbSet<TicketEntity> Tickets { get; set; }
        public DbSet<ReplyEntity> Replies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TicketEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ReplyEntityConfiguration());
        }
    }
}
