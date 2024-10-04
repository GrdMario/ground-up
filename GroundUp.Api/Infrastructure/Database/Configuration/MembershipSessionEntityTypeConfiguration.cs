namespace GroundUp.Api.Infrastructure.Database.Configuration
{
    using GroundUp.Api.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class MembershipSessionEntityTypeConfiguration : IEntityTypeConfiguration<MembershipSession>
    {
        public void Configure(EntityTypeBuilder<MembershipSession> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.IsCompleted).IsRequired();
            builder.Property(p =>p.IsCancelled).IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.Comment);
            builder.Property(p => p.Start);
            builder.Property(p => p.End);
        }
    }
}
