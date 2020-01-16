using Microsoft.AspNetCore.Builder;

namespace SpendShelf.BankTransactionsImport.Infrastructure.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
