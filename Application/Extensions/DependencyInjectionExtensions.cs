using Application.Attachments;
using Application.Comments;
using Application.Files;
using Application.Users;
using Application.Tickets;
using Application.Drafts;
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

            return services;
        }
    }
}
