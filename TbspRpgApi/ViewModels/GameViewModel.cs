using System;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; }
        public Guid AdventureId { get; }
        public Guid UserId { get; }

        public GameViewModel(Game game)
        {
            Id = game.Id;
            AdventureId = game.AdventureId;
            UserId = game.UserId;
        }
    }
}