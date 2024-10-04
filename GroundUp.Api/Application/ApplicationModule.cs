namespace GroundUp.Api.Application
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Services;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IMembershipSessionService, MembershipSessionService>();
            services.AddScoped<IMembershipTypeService, MembershipTypeService>();
            services.AddScoped<ICalendarService, CalendarService>();
            return services;
        }
    }
}
