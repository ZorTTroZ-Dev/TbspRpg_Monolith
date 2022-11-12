using System;

namespace TbspRpgDataLayer.Entities;

public class AdventureObjectGameState
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid AdventureObjectId { get; set; }
    public string AdventureObjectState { get; set; }
    
    public Game Game { get; set; }
    public AdventureObject AdventureObject { get; set; }
}