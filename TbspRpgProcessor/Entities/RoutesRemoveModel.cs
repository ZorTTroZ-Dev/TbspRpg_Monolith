using System;
using System.Collections.Generic;

namespace TbspRpgProcessor.Entities;

public class RoutesRemoveModel
{
    public ICollection<Guid> CurrentRouteIds { get; set; }
    public Guid LocationId { get; set; }
}