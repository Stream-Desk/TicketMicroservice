using Application.Attachments;
using Application.Comments;
using Application.Users;
using Application.Tickets;
using Application.Drafts;
using Application.Files;
using Application.Statuses;
using Domain.Statuses;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITicketService, TicketsService>();
            services.AddScoped<IDraftService, DraftsService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IStatusService, StatusService>();
            
            return services;
        }
    }
}
