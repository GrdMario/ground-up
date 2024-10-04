namespace GroundUp.Api.Infrastructure.Database.Internal
{
    using Microsoft.EntityFrameworkCore;
    using System.IO;
    using System;
    using System.Reflection;

    public class GroundUpContext : DbContext
    {
        public GroundUpContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
