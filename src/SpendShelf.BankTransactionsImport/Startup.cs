using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SpendShelf.BankTransactionsImport.Infrastructure;
using SpendShelf.BankTransactionsImport.Infrastructure.Extensions;
using SpendShelf.BankTransactionsImport.TransactionsExport;
using SpendShelf.BankTransactionsImport.TransactionsParser;
using SpendShelf.BankTransactionsImport.TransactionsProcessor;

namespace SpendShelf.BankTransactionsImport
{
    /// <summary>
    /// Startup allows registering app dependencies and configure a middleware.
    /// </summary>
    public sealed class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </summary>
        /// <param name="services">Service collection.</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilogLogging();
            services.AddControllers();
            services.AddCors();
            services.AddSingleton<IBankTransactionsXlsParser, BankTransactionsXlsParser>();
            services.AddSingleton<IBankTransactionsDateParser, BankTransactionsDateParser>();
            services.AddChannel();
            services.Configure<MqttClientConfig>(x =>
            {
                var url = Environment.GetEnvironmentVariable("SPENDSHELF_MQTT_HOST");
                if (string.IsNullOrEmpty(url))
                {
                    throw new KeyNotFoundException("SPENDSHELF_MQTT_HOST not found");
                }

                x.ServerUrl = url;
            });
            services.AddMqttClient();
            services.AddSingleton<ITransactionsExporter, TransactionsExporter>();
            services.AddHostedService<TransactionProcessorHostedService>();

            // IServiceCollection services
            // This needed to fix https://github.com/ExcelDataReader/ExcelDataReader/issues/241
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        } 

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Middleware builder.</param>
        public static void Configure(IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
            app.UseCustomExceptionMiddleware();
            app.UseCors();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
