using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Infrastructure.Exceptions;
using System.Net;
using System.Text.Json;

namespace AdvancedDevSample.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception attrapée : {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new { error = exception.Message };

            switch (exception)
            {
                case DomainException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                    break;
                case ApplicationServiceExceptions:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
                case InfrastructureException:
                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable; // 503
                    response = new { error = "Erreur système. Réessayez plus tard." };
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                    response = new { error = "Une erreur inattendue est survenue." };
                    break;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
