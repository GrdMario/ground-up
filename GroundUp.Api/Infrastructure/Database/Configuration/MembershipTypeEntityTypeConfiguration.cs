namespace GroundUp.Api.Infrastructure.Database.Configuration
{
    using GroundUp.Api.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class MembershipTypeEntityTypeConfiguration : IEntityTypeConfiguration<MembershipType>
    {
        public void Configure(EntityTypeBuilder<MembershipType> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.Color).IsRequired();
        }
    }
}
