using System;

namespace TbspRpgProcessor.Entities;

public class GameStartModel
{
    public Guid UserId { get; set; }
    public Guid AdventureId { get; set; }
    public DateTime TimeStamp { get; set; }
}