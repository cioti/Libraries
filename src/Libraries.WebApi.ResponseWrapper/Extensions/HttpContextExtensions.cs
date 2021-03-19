using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Libraries.WebApi.ResponseWrapper.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsSuccess(this HttpContext context)
            => context.Response.StatusCode >= 200 && context.Response.StatusCode < 400;

        public static bool IsWrapIgnored(this HttpContext context)
            => context.Response.Headers.Any(h => h.Key == Constants.IgnoreWrap);

        public static bool IsWrapSkipped(this HttpContext context)
            => context.Response.StatusCode == StatusCodes.Status204NoContent ||
            context.Response.StatusCode == StatusCodes.Status304NotModified;
    }
}
