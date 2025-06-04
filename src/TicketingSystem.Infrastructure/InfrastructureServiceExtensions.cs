using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Infrastructure.Persistence;
using TicketingSystem.Infrastructure.Repositories;

namespace TicketingSystem.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddDbContext<TicketingDbContext>(options => options.UseInMemoryDatabase("assestment"));

            return services;
        }
    }

}
