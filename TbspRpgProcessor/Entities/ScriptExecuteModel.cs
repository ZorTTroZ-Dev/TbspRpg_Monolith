using System;
using TbspRpgDataLayer.Entities;

namespace TbspRpgProcessor.Entities;

public class ScriptExecuteModel
{
    public Guid ScriptId { get; set; }
    public Guid GameId { get; set; }
    public Script Script { get; set; }
    public Game Game { get; set; }
}