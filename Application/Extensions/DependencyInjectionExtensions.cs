using Application.Tickets;
using Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ITicketService, TicketsService>();

            return services;
        }
    }
}
