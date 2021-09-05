using TbspRpgApi.RequestModels;
using TbspRpgDataLayer.ArgumentModels;

namespace TbspRpgApi.Adapters
{
    public static class AdventureFilterAdapter
    {
        public static AdventureFilter ToDataLayerFilter(AdventureFilterRequest filter)
        {
            return new AdventureFilter()
            {
                CreatedBy = filter.CreatedBy
            };
        }
    }
}