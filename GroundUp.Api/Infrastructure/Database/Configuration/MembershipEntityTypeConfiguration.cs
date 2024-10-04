namespace GroundUp.Api.Infrastructure.Database.Configuration
{
    using GroundUp.Api.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class MembershipEntityTypeConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.From).IsRequired();

            builder.Property(p => p.To).IsRequired();

            builder.Property(p => p.ClientId).IsRequired();

            builder.Property(p => p.SessionCount).IsRequired();

            builder.HasOne(p => p.Client).WithMany(p => p.Memberships).HasForeignKey(fk => fk.ClientId);

            builder.HasOne(p => p.MembershipType).WithMany().HasForeignKey(fk => fk.MembershipTypeId);

            builder
                .HasMany(p => p.MembershipSessions)
                .WithOne(p => p.Membership)
                .HasForeignKey(fk => fk.MembershipId)
                .HasPrincipalKey(pk => pk.Id);
        }
    }
}
