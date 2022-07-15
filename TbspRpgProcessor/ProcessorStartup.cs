using Microsoft.Extensions.DependencyInjection;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor
{
    public class ProcessorStartup
    {
        public static void InitializeProcessorLayer(IServiceCollection services)
        {
            // processors
            services.AddScoped<IGameProcessor, GameProcessor>();
            services.AddScoped<IContentProcessor, ContentProcessor>();
            services.AddScoped<IMapProcessor, MapProcessor>();
            services.AddScoped<ILocationProcessor, LocationProcessor>();
            services.AddScoped<IRouteProcessor, RouteProcessor>();
            services.AddScoped<IAdventureProcessor, AdventureProcessor>();
            services.AddScoped<ITbspRpgProcessor, TbspRpgProcessor>();
            
            services.AddScoped<IMailClient, MailClient>();
        }
    }
}