using System;
using TbspRpgDataLayer.Entities;

namespace TbspRpgProcessor.Entities;

public class SourceForKeyModel
{
    public Guid Key { get; set; }
    public Guid AdventureId { get; set; }
    public string Language { get; set; }
    public bool Processed { get; set; }
    public Game Game { get; set; }
}