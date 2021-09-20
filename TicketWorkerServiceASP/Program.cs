using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketWorkerServiceASP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<Worker>();
                    //services.AddHostedService<TimedHostedService>();
                    //services.AddHostedService<ConsumeScopedServiceHostedService>();
                    //services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
                    services.AddSingleton<MonitorLoop>();
                    services.AddHostedService<QueuedHostedService>();
                    services.AddSingleton<IBackgroundTaskQueue>(ctx => {
                        if (!int.TryParse(hostContext.Configuration["QueueCapacity"], out var queueCapacity))
                            queueCapacity = 100;
                        return new BackgroundTaskQueue(queueCapacity);
                    });

                });
    }
}
