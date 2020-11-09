using DakarRally.Middleware;
using Microsoft.AspNetCore.Builder;


namespace DakarRally.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
     
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
