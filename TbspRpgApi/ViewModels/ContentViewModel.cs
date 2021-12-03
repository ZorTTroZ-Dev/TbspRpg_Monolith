using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class ContentViewModel
    {
        public ContentViewModel(IEnumerable<Content> contents) {
            SourceKeys = new List<Guid>();
            Index = 0;
            foreach (var content in contents)
            {
                Id = content.GameId;
                if(content.Position > Index)
                    Index = content.Position;
                SourceKeys.Add(content.SourceKey);
            }
        }

        public ContentViewModel(Content content)
        {
            SourceKeys = new List<Guid>();
            Id = content.GameId;
            Index = content.Position;
            SourceKeys.Add(content.SourceKey);
        }
        
        public Guid Id { get; }

        public List<Guid> SourceKeys { get; }

        public ulong Index { get; }
    }
}