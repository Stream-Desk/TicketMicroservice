using System.Text.Json.Serialization;
using Application.Settings;
using Application.Extensions;
using Application.Mail;
using Database.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Infrastracture;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            services.AddCors(options =>
            {
                // options.AddPolicy(MyAllowSpecificOrigins,
                //                   builder =>
                //                   { builder.WithOrigins(
                //                       "https://streamdesk-webapp.herokuapp.com", 
                //                       "https://backoffice-interface.herokuapp.com", 
                //                       "https://laboremus-supportservice.herokuapp.com",
                //                       "http://localhost:8080", 
                //                       "http://localhost:8082", 
                //                       "https://8082-scarlet-blackbird-ylncjra7.ws-eu15.gitpod.io/")
                //                        .AllowAnyHeader()
                //                        .AllowAnyMethod();
                //                   });
                
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader().AllowAnyMethod();
                    });
                });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddDataBaseLayer();
            services.AddApplicationLayer();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//, ApplicationDBContext context, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            // app.UseCors(MyAllowSpecificOrigins);
            app.UseCors();
            app.UseAuthorization();
            //DbSeeder.SeedDb(context,userManager);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
