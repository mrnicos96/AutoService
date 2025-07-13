// AutoService.API/Middleware/LoggingMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AutoService.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation($"Starting request: {context.Request.Method} {context.Request.Path}");

                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation($"Completed request: {context.Request.Method} {context.Request.Path} " +
                                      $"in {stopwatch.ElapsedMilliseconds}ms " +
                                      $"with status {context.Response.StatusCode}");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, $"Request error: {context.Request.Method} {context.Request.Path} " +
                                    $"after {stopwatch.ElapsedMilliseconds}ms");
                throw;
            }
        }
    }
}