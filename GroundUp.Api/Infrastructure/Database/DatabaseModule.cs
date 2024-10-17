namespace GroundUp.Api.Infrastructure.Database
{
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using GroundUp.Api.Infrastructure.Database.Internal;
    using GroundUp.Api.Infrastructure.Database.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DatabaseModule
    {
        public static IServiceCollection AddDatabaseModule(this IServiceCollection services)
        {
            services.AddDbContext<GroundUpContext>(opt =>
            {
                opt.UseSqlite("Data Source=C:\\data\\GroundUp\\groundup.db");
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMembershipSessionRepository, MembershipSessionRepository>();
            services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();

            return services;
        }
    }
}
