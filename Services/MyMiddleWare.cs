using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CrediBill_ASP.Services
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MyMiddleWare
    {
        private readonly RequestDelegate _next;

        public MyMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddleWareExtensions
    {
        public static IApplicationBuilder UseMyMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleWare>();
        }
    }
}