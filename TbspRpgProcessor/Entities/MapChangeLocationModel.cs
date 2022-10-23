using System;

namespace TbspRpgProcessor.Entities;

public class MapChangeLocationModel
{
    public Guid GameId { get; set; }
    public Guid RouteId { get; set; }
    public DateTime TimeStamp { get; set; }
}