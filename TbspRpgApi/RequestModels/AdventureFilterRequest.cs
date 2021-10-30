using System;
using TbspRpgDataLayer.ArgumentModels;

namespace TbspRpgApi.RequestModels
{
    public class AdventureFilterRequest
    {
        public Guid CreatedBy { get; set; }

        public AdventureFilter ToAdventureFilter()
        {
            return new AdventureFilter()
            {
                CreatedBy = CreatedBy
            };
        }
    }
}