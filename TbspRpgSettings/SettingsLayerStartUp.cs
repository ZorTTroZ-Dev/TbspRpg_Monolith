using Microsoft.Extensions.DependencyInjection;

namespace TbspRpgSettings;

public static class SettingsLayerStartUp
{
    public static void InitializeSettingsLayer(IServiceCollection services)
    {
        services.AddSingleton<TbspRpgUtilities>();
    }
}