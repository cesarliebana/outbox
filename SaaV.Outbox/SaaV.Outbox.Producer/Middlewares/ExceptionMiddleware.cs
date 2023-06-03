using SaaV.Outbox.Producer.Exceptions;
using System.Net;
using System.Text.Json;
using System.Transactions;

namespace SaaV.Outbox.Producer.Middlewares
{
    public record ErrorDetails(int Code, string Message)
    {
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorDetails errorDetails;
            context.Response.ContentType = "application/json";            
            
            switch (exception)
            {
                case OutboxTransactionException:
                    _logger.LogError(exception, exception.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorDetails = new ErrorDetails(500, exception.Message);
                    break;
                
                default:
                    _logger.LogError(exception, exception.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorDetails = new ErrorDetails(500, exception.Message);
                    break;
            }
            await context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
