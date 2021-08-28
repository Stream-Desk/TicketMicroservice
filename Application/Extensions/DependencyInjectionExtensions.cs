﻿using Application.Comments;
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
            return services;
        }
    }
}
