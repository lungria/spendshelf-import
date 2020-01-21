using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SpendShelf.BankTransactionsImport.Infrastructure.Extensions;
using SpendShelf.BankTransactionsImport.TransactionsParser;
using SpendShelf.BankTransactionsImport.TransactuinsProcessor;

namespace SpendShelf.BankTransactionsImport
{
    /// <summary>
    /// Startup allows to register app dependencies and configure middleware.
    /// </summary>
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
