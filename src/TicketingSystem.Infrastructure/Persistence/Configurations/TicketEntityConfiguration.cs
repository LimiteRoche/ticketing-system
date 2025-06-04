using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Infrastructure.Persistence.Entities;

namespace TicketingSystem.Infrastructure.Persistence.Configurations
{
    public class TicketEntityConfiguration : IEntityTypeConfiguration<TicketEntity>
    {
        public void Configure(EntityTypeBuilder<TicketEntity> entity)
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id)
                .IsRequired();

            entity.Property(t => t.UserId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(t => t.Username)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(t => t.Subject)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(t => t.Description)
                .HasMaxLength(2000)
                .IsRequired();

            entity.Property(t => t.AvatarUrl)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(t => t.Status)
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.HasMany(t => t.Replies)
                .WithOne(r => r.Ticket)
                .HasForeignKey(r => r.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
