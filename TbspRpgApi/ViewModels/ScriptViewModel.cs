using System;
using System.Collections.Generic;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels;

public class ScriptViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public List<ScriptViewModel> Includes { get; set; }
    public Guid AdventureId { get; set; }

    public ScriptViewModel() { }

    public ScriptViewModel(Script script)
    {
        Id = script.Id;
        Name = script.Name;
        Type = script.Type;
        Content = script.Content;
        AdventureId = script.AdventureId;
        if (script.Includes != null)
        {
            Includes = new List<ScriptViewModel>();
            foreach (var include in script.Includes)
            {
                Includes.Add(new ScriptViewModel(include));
            }
        }
    }

    public Script ToEntity()
    {
        var includeEntities = new List<Script>();
        foreach (var include in Includes)
        {
            includeEntities.Add(include.ToEntity());
        }
        return new Script()
        {
            Id = Id,
            Name = Name,
            AdventureId = AdventureId,
            Content = Content,
            Type = Type,
            Includes = includeEntities
        };
    }
}