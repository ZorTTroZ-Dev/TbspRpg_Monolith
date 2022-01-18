using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace TbspRpgApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var builder = WebApplication.CreateBuilder(args);
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(name: "AllowAll",
                        configurePolicy =>
                        {
                            configurePolicy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        });
                });
                builder.Host.UseSerilog();
                var startup = new Startup(builder.Configuration);
                startup.ConfigureServices(builder.Services);
                var app = builder.Build();
                startup.Configure(app, app.Environment);
                app.UseCors("AllowAll");
                app.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
