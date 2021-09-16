using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Coravel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Formatting.Json;

namespace TicketWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), "logs/log.txt",restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information,
                rollingInterval:RollingInterval.Day)
                .WriteTo.File("logs/errorlog.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();

            try
            {
                Log.Information("Starting our service...");
                var host = CreateHostBuilder(args).Build();
                host.Services.UseScheduler(scheduler =>
                {
                    var jobSchedule = scheduler.Schedule<ProcessTicket>();
                    jobSchedule.EverySeconds(2);
                });

                host.Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Exception in application");
            }
            finally
            {
                Log.Information("Exiting our service...");
                Log.CloseAndFlush();
            }
           
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ITicketConnector, TicketQueueConnector>();
                    services.AddScheduler();
                    services.AddTransient<ProcessTicket>();
                });
        }
    }
}
