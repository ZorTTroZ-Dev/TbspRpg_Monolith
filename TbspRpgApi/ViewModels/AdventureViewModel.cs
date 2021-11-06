using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class AdventureViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SourceKey { get; set; }
        public Guid CreatedByUserId { get; set; }

        public AdventureViewModel() { }

        public AdventureViewModel(Adventure adventure)
        {
            Id = adventure.Id;
            Name = adventure.Name;
            SourceKey = adventure.SourceKey;
            CreatedByUserId = adventure.CreatedByUserId;
        }

        public Adventure ToEntity()
        {
            return new Adventure()
            {
                Id = Id,
                Name = Name,
                SourceKey = SourceKey,
                CreatedByUserId = CreatedByUserId
            };
        }
    }
}