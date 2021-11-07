using System;
using TbspRpgApi.Entities;

namespace TbspRpgProcessor.Entities
{
    public class AdventureUpdateModel
    {
        public Adventure Adventure { get; set; }
        public Source Source { get; set; }
        public Guid UserId { get; set; }
        public string Language { get; set; }
    }
}