using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace Middleware
{
    public class ApiResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);

            context.Response.Body = originalBodyStream;

            if (!string.IsNullOrWhiteSpace(responseBody) && context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
            {
                object wrappedResponse;
                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    wrappedResponse = new { success = true, data = JsonSerializer.Deserialize<object>(responseBody) };
                }
                else
                {
                    wrappedResponse = new { success = false, error = JsonSerializer.Deserialize<object>(responseBody) };
                }
                var json = JsonSerializer.Serialize(wrappedResponse);
                context.Response.ContentLength = json.Length;
                await context.Response.WriteAsync(json);
            }
            else
            {
                await context.Response.WriteAsync(responseBody);
            }
        }
    }

    public static class ApiResponseWrapperMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiResponseWrapperMiddleware>();
        }
    }
}
