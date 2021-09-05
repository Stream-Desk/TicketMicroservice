using Database.Collections;
using Domain.Comments;
using Domain.Tickets;
using Domain.Drafts;
using Domain.Users;
using Domain.Registrations;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDataBaseLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserCollection, UsersCollection>();
            services.AddScoped<ITicketCollection, TicketsCollection>();
            services.AddScoped<ICommentsCollection, CommentsCollection>();
            services.AddScoped<IDraftCollection, DraftsCollection>();
            services.AddScoped<IRegistrationCollection, RegistrationsCollection>();
            return services;
        }
    }
}