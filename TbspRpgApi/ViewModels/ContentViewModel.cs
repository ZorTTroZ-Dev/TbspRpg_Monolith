using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class ContentViewModel
    {
        public ContentViewModel(IEnumerable<Content> contents) {
            SourceIds = new List<Guid>();
            Index = 0;
            foreach (var content in contents)
            {
                Id = content.GameId;
                if(content.Position > Index)
                    Index = content.Position;
                SourceIds.Add(content.SourceId);
            }
        }

        public ContentViewModel(Content content)
        {
            SourceIds = new List<Guid>();
            Id = content.GameId;
            Index = content.Position;
            SourceIds.Add(content.SourceId);
        }
        
        public Guid Id { get; }

        public List<Guid> SourceIds { get; }

        public ulong Index { get; }
    }
}