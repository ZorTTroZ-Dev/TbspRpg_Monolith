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

    public ScriptViewModel(Script script)
    {
        Id = script.Id;
        Name = script.Name;
        Type = script.Type;
        Content = script.Type;
        if (script.Includes != null)
        {
            Includes = new List<ScriptViewModel>();
            foreach (var include in script.Includes)
            {
                Includes.Add(new ScriptViewModel(include));
            }
        }
    }
}