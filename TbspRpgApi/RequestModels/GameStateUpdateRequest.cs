using System;

namespace TbspRpgApi.RequestModels;

public class GameStateUpdateRequest
{
    public Guid GameId { get; set; }
    public string GameState { get; set; }
}