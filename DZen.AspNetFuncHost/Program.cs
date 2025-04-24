using DZen.AspNetFuncHost;
using DZen.AspNetFuncHost.Sample.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
        .ConfigureFunctionsWebApplication()
        .ConfigureServices(services =>
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();

            services.AddASPNetApplication(args, services =>
            {
                services.AddLogging();
                IMvcBuilder mvcBuilder = services.AddControllers();
                mvcBuilder.AddApplicationPart(typeof(WeatherForecastController).Assembly);
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            },
            app =>
            {
                //if (env.IsDevelopment())
                //{
                //    app.UseSwagger();
                //    app.UseSwaggerUI();
                //}

                app.UseRouting();
                //app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        }).Build();

        host.Run();
    }
}
