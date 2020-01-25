using System;
using System.IO;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace SpendShelf.BankTransactionsImport.Infrastructure.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddSerilogLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .CreateLogger();

            services.AddSingleton<ILogger>(Log.Logger);
            return services;
        }

        internal static IServiceCollection AddChannel(this IServiceCollection services)
        {
            services.AddSingleton<Channel<Stream>>(Channel.CreateUnbounded<Stream>(new UnboundedChannelOptions() { SingleReader = true }));
            services.AddSingleton<ChannelReader<Stream>>(svc => svc.GetRequiredService<Channel<Stream>>().Reader);
            services.AddSingleton<ChannelWriter<Stream>>(svc => svc.GetRequiredService<Channel<Stream>>().Writer);
            return services;
        }

        internal static IServiceCollection AddMqttClient(this IServiceCollection services)
        {
            return services.AddSingleton<IMqttClient>(new MqttFactory().CreateMqttClient());
        }
    }
}