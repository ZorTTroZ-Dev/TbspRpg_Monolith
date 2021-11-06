using TbspRpgApi.Entities;

namespace TbspRpgProcessor.Entities
{
    public class AdventureUpdateModel
    {
        public Adventure adventure { get; set; }
        public Source source { get; set; }
        public string language { get; set; }
    }
}