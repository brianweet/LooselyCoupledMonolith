﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(collection =>
                {
                    collection.AddHostedService<MessageProcessorBackgroundService>();
                });
    }

    class MessageProcessorBackgroundService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This is the top level that would be using a message broker and dispatching
            // messages to the relevant handlers.
            return Task.CompletedTask;
        }
    }
}
