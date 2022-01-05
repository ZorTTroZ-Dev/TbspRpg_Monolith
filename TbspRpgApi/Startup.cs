using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;
using TbspRpgDataLayer;
using TbspRpgProcessor;
using TbspRpgSettings.Settings;

namespace TbspRpgApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            // settings objects
            services.Configure<SmtpSettings>(Configuration.GetSection("Smtp"));
            services.AddSingleton<ISmtpSettings>(sp =>
                sp.GetRequiredService<IOptions<SmtpSettings>>().Value);
            
            services.Configure<DatabaseSettings>(Configuration.GetSection("Database"));
            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.AddSingleton<IJwtSettings>(sp =>
                sp.GetRequiredService<IOptions<JwtSettings>>().Value);
            
            DataLayerStartUp.InitializeDataLayer(services);
            ProcessorStartup.InitializeProcessorLayer(services);

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAdventuresService, AdventuresService>();
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<IContentsService, ContentsService>();
            services.AddScoped<IMapsService, MapsService>();
            services.AddScoped<ILocationsService, LocationsService>();
            services.AddScoped<ISourcesService, SourcesService>();
            services.AddScoped<IRoutesService, RoutesService>();
            services.AddScoped<IPermissionService, PermissionService>();

            // swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TbspRpgApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TbspRpgApi v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
