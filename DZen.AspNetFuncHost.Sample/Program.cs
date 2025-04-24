
using System.Reflection;

namespace DZen.AspNetFuncHost.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplication app = BuildWebApp(args);
            ConfigureWebApplication(app);
            app.Run();
        }

        public static WebApplication BuildWebApp(string[] args, IEnumerable<Assembly> applicationPartAssemblies = default)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            IMvcBuilder mvcBuilder = builder.Services.AddControllers();
            if (applicationPartAssemblies is { })
            {
                foreach (Assembly assembly in applicationPartAssemblies)
                {
                    mvcBuilder.AddApplicationPart(assembly);
                }
            }
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            return app;
        }

        public static WebApplication ConfigureWebApplication(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Map("/cool", () =>
            {

            });
            return app;
        }
    }
}
