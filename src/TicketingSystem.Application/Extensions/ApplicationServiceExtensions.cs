using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TicketingSystem.Application.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITicketService, TicketService>();
            return services;
        }
    }

}
