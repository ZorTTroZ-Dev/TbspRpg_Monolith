using System;
using System.Collections.Generic;

namespace TbspRpgDataLayer.Entities;

public class Script
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    
    public ICollection<Script> IncludedIn { get; set; }
    public ICollection<Script> Includes { get; set; }
}