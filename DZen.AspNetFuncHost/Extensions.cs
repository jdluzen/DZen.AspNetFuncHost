using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DZen.AspNetFuncHost
{
    public static class Extensions
    {
        public static IServiceCollection AddASPNetApplication(this IServiceCollection functionServices, string[] args, Action<IServiceCollection> configureServices, Action<IApplicationBuilder> appConfig, IEnumerable<Assembly> applicationParts = default, WebApplicationBuilder webAppBuilder = default, IServiceProvider provider = default)
        {
            webAppBuilder ??= WebApplication.CreateBuilder(args);
            configureServices(webAppBuilder.Services);

            IApplicationBuilder appBuilder = webAppBuilder.Build();
            appConfig(appBuilder);

            RequestDelegate reqDel = appBuilder.Build();
            functionServices.AddSingleton(reqDel);
            return functionServices;
        }
    }
}
