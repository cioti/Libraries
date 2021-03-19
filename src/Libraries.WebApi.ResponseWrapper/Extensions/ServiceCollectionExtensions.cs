using Libraries.WebApi.ResponseWrapper.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Libraries.WebApi.ResponseWrapper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddResponseWrapper(this IServiceCollection services) => services.AddResponseWrapper(null);
        public static void AddResponseWrapper(this IServiceCollection services, Action<WrapperOptions> configureOptions)
        {
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            services.AddSingleton<IResponseOperations, ResponseOperations>();
        }

        public static void UseResponseWrapper(this IApplicationBuilder app)
        {
            app.UseMiddleware<ResponseWrapperMiddleware>();
        }
    }
}
