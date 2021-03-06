using Microsoft.Extensions.DependencyInjection;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor
{
    public class ProcessorStartup
    {
        public static void InitializeProcessorLayer(IServiceCollection services)
        {
            // services and repositories
            services.AddScoped<IGameProcessor, GameProcessor>();
            services.AddScoped<IContentProcessor, ContentProcessor>();
            services.AddScoped<IMapProcessor, MapProcessor>();
            services.AddScoped<ILocationProcessor, LocationProcessor>();
            services.AddScoped<IRouteProcessor, RouteProcessor>();
            services.AddScoped<ISourceProcessor, SourceProcessor>();
            services.AddScoped<IAdventureProcessor, AdventureProcessor>();
            services.AddScoped<IUserProcessor, UserProcessor>();
            services.AddScoped<IScriptProcessor, ScriptProcessor>();
            
            services.AddScoped<IMailClient, MailClient>();
        }
    }
}