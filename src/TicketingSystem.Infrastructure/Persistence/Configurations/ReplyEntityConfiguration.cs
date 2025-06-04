using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingSystem.Infrastructure.Persistence.Entities;

namespace TicketingSystem.Infrastructure.Persistence.Configurations
{
    public class ReplyEntityConfiguration : IEntityTypeConfiguration<ReplyEntity>
    {
        public void Configure(EntityTypeBuilder<ReplyEntity> entity)
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id)
                .IsRequired();

            entity.Property(r => r.TicketId)
                .IsRequired();

            entity.Property(r => r.Message)
                .HasMaxLength(2000)
                .IsRequired();

            entity.Property(r => r.AgentName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(t => t.AvatarUrl)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(r => r.CreatedAt)
                .IsRequired();
        }
    }
}
