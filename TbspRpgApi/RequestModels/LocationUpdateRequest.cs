using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class LocationUpdateRequest
    {
        public LocationViewModel location { get; set; }
        public SourceViewModel source { get; set; }

        public LocationUpdateModel ToLocationUpdateModel()
        {
            return new LocationUpdateModel()
            {
                Location = location.ToEntity(),
                Source = source.ToEntity(),
                Language = source.Language
            };
        }
    }
}