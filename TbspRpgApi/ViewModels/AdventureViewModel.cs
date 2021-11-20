using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class AdventureViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid InitialSourceKey { get; set; }
        public Guid DescriptionSourceKey { get; set; }
        public Guid CreatedByUserId { get; set; }

        public AdventureViewModel() { }

        public AdventureViewModel(Adventure adventure)
        {
            Id = adventure.Id;
            Name = adventure.Name;
            InitialSourceKey = adventure.InitialSourceKey;
            DescriptionSourceKey = adventure.DescriptionSourceKey;
            CreatedByUserId = adventure.CreatedByUserId;
        }

        public Adventure ToEntity()
        {
            return new Adventure()
            {
                Id = Id,
                Name = Name,
                InitialSourceKey = InitialSourceKey,
                DescriptionSourceKey = DescriptionSourceKey,
                CreatedByUserId = CreatedByUserId
            };
        }
    }
}