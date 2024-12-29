namespace SteamTradeHelper.API.Infrastructure.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestAuditMiddleware>();
        }
    }
}
