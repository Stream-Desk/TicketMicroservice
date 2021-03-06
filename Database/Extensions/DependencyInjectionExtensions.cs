using Database.Collections;
using Domain.Comments;
using Domain.Files;
using Domain.Tickets;
using Domain.Drafts;
using Domain.Statuses;
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
            services.AddScoped<ICommentsCollection, CommentsCollection>();
            services.AddScoped<IDraftCollection, DraftsCollection>();
            services.AddScoped<IFileCollection, FileCollection>();
            services.AddScoped<IStatusCollection, StatusCollection>();
            
            return services;
        }
    }
}