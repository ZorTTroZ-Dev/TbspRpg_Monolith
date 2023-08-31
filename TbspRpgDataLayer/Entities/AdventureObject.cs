﻿using System;
using System.Collections.Generic;

namespace TbspRpgDataLayer.Entities;

public class AdventureObject
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid NameSourceKey { get; set; }
    public Guid DescriptionSourceKey { get; set; }
    public Guid AdventureId { get; set; }
    public Guid? InitializationScriptId { get; set; }
    public Guid? ActionScriptId { get; set; }

    public Adventure Adventure { get; set; }
    public Script InitializationScript { get; set; }
    public Script ActionScript { get; set; }
    public ICollection<Location> Locations { get; set; }
    public ICollection<AdventureObjectGameState> AdventureObjectGameStates { get; set; }

    public override bool Equals(object obj)
    {
        var adventureObject = obj as AdventureObject;
        if (adventureObject == null)
            return false;
        return Id == adventureObject.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}