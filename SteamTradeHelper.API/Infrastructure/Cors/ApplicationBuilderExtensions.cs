namespace SteamTradeHelper.API.Infrastructure.Cors
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomCors(this IApplicationBuilder app)
        {
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });
        }
    }
}
