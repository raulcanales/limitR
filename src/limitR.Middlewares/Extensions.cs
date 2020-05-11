using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace limitR.Middlewares
{
    public static class Extensions
    {
        public static IServiceCollection AddRateLimiterPerRequests(this IServiceCollection services)
        {
            /*services.Configure<RequestRateLimiterSettings>(pe => new RequestRateLimiterSettings
            {
                ApiKeyHeader = "apiKey"
            });*/
            services.AddSingleton(new RequestRateLimiterSettings
            {
                ApiKeyHeader = "apiKey"
            });
            var rrl = new RequestRateLimiter(2, TimeSpan.FromSeconds(5));
            services.AddSingleton<IRequestRateLimiter>(rrl);
            return services;
        }

        public static IApplicationBuilder UseRequestRateLimiter(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestRateLimiterMiddleware>();
            return app;
        }
    }
}
