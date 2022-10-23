using System.Collections.Generic;
using TbspRpgDataLayer.Entities;

namespace TbspRpgProcessor.Entities;

public class GamesRemoveModel
{
    public ICollection<Game> Games { get; set; }
    public bool Save { get; set; } = true;
}