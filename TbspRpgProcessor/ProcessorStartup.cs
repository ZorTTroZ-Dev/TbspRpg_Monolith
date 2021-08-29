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
        }
    }
}