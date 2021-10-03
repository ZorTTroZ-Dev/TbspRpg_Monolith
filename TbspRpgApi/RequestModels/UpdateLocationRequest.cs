using TbspRpgApi.ViewModels;

namespace TbspRpgApi.RequestModels
{
    public class UpdateLocationRequest
    {
        public LocationViewModel location { get; set; }
        public SourceViewModel source { get; set; }
    }
}