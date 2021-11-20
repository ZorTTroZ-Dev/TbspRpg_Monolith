using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class AdventureUpdateRequest
    {
        public AdventureViewModel adventure { get; set; }
        public SourceViewModel initialSource { get; set; }
        public SourceViewModel descriptionSource { get; set; }

        public AdventureUpdateModel ToAdventureUpdateModel()
        {
            return new AdventureUpdateModel()
            {
                Adventure = adventure.ToEntity(),
                InitialSource = initialSource.ToEntity(),
                DescriptionSource = descriptionSource.ToEntity(),
                Language = initialSource.Language
            };
        }
    }
}