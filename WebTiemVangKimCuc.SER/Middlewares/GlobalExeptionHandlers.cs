using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebTiemVangKimCuc.SER.Middlewares
{
    public class GlobalExeptionHandlers : IMiddleware
    {
        private readonly ILogger<GlobalExeptionHandlers> _logger;
        public GlobalExeptionHandlers(ILogger<GlobalExeptionHandlers> logger)
        {
            _logger = logger;
        }

        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception from {nameof(GlobalExeptionHandlers)} with content: {ex.Message}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "Server Error",
                    Detail = "An internal server error has occured"
                };

                //string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsJsonAsync(problem);

                context.Response.ContentType = "application/json";
            }
        }

    }
}
