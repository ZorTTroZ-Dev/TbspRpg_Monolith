using System;
using TbspRpgDataLayer.ArgumentModels;

namespace TbspRpgApi.RequestModels
{
    public class GameFilterRequest
    {
        public Guid AdventureId { get; set; }
        public Guid UserId { get; set; }

        public GameFilter ToGameFilter()
        {
            return new GameFilter()
            {
                AdventureId = AdventureId,
                UserId = UserId
            };
        }
    }
}