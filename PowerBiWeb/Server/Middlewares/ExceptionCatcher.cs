using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PowerBiWeb.Server.Utilities.Exceptions;
using System.Net;
using System.Threading.Tasks;

namespace PowerBiWeb.Server.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionCatcher
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionCatcher> _logger;

        public ExceptionCatcher(RequestDelegate next, ILogger<ExceptionCatcher> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (MessageException ex)
            {
                _logger.LogError(ex, string.Empty);

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(ex.ExcptMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync("General error");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionCatcherExtensions
    {
        public static IApplicationBuilder UseExceptionCatcher(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionCatcher>();
        }
    }
}
