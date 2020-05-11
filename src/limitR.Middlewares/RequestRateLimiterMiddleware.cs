using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace limitR.Middlewares
{

    public class RequestRateLimiterMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestRateLimiterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRequestRateLimiter requestRateLimiter, RequestRateLimiterSettings settings)
        {
            if (!context.Request.Headers.ContainsKey(settings.ApiKeyHeader))
                throw new Exception();

            var apiKey = context.Request.Headers[settings.ApiKeyHeader];

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                if (!requestRateLimiter.CanPerformRequest(apiKey))
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync("Too many requests");
                }
            }

            await _next(context);
        }
    }
}
