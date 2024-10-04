namespace GroundUp.Api.Infrastructure.Database.Configuration
{
    using GroundUp.Api.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Email).IsRequired();

            builder.Property(p => p.PhoneNumber).IsRequired();

            builder.Property(p => p.FirstName).IsRequired();

            builder.Property(p => p.LastName).IsRequired();

            builder.Property(p => p.DateOfBirth).IsRequired();

            builder.Property(p => p.Address).IsRequired();

            builder.Property(p => p.City).IsRequired();

            builder.HasMany(p => p.Memberships).WithOne().HasForeignKey(fk => fk.ClientId);
        }
    }
}
