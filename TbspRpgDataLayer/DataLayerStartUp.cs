using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;

namespace TbspRpgDataLayer
{
    public class DataLayerStartUp
    {
        public static void InitializeDataLayer(IServiceCollection services)
        {
            // services and repositories
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ISourcesRepository, SourcesRepository>();
            services.AddScoped<IRoutesRepository, RoutesRepository>();
            services.AddScoped<ILocationsRepository, LocationsRepository>();
            services.AddScoped<IAdventuresRepository, AdventuresRepository>();
            
            // setup the database connection
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            services.AddDbContext<DatabaseContext>(
                options => options.UseNpgsql(connectionString)
            );
        }
    }
}