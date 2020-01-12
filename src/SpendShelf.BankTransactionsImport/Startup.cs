using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SpendShelf.BankTransactionsImport.TransactionsParser;

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
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<IBankTransactionsXlsParser, BankTransactionsXlsParser>();
            services.AddSingleton<IBankTransactionsDateParser, BankTransactionsDateParser>();

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
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!").ConfigureAwait(false); });
            });
        }
    }
}
