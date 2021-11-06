using TbspRpgApi.ViewModels;

namespace TbspRpgApi.RequestModels
{
    public class LocationUpdateRequest
    {
        public LocationViewModel location { get; set; }
        public SourceViewModel source { get; set; }
    }
}