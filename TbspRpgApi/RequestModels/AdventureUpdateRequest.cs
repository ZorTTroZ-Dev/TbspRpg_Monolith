using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class AdventureUpdateRequest
    {
        public AdventureViewModel adventure { get; set; }
        public SourceViewModel source { get; set; }

        public AdventureUpdateModel ToAdventureUpdateModel()
        {
            return new AdventureUpdateModel()
            {
                Adventure = adventure.ToEntity(),
                Source = source.ToEntity(),
                Language = source.Language
            };
        }
    }
}