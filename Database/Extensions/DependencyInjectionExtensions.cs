using Database.Collections;
using Domain.Tickets;
using Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDataBaseLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserCollection, UsersCollection>();
            services.AddScoped<ITicketCollection, TicketsCollection>();
           
            return services;
        }
    }
}