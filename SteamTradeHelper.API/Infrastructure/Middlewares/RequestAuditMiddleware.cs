namespace SteamTradeHelper.API.Infrastructure.Middlewares
{
    public class RequestAuditMiddleware(RequestDelegate next, ILogger<RequestAuditMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<RequestAuditMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Entering RequestAuditMiddleware...");
            await _next(context);
        }
    }
}
