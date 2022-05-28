using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class AdventureViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid InitialSourceKey { get; set; }
        public Guid DescriptionSourceKey { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime PublishDate { get; set; }
        public Guid? InitializationScriptId { get; set; }
        public Guid? TerminationScriptId { get; set; }

        public AdventureViewModel() { }

        public AdventureViewModel(Adventure adventure)
        {
            Id = adventure.Id;
            Name = adventure.Name;
            InitialSourceKey = adventure.InitialSourceKey;
            DescriptionSourceKey = adventure.DescriptionSourceKey;
            CreatedByUserId = adventure.CreatedByUserId;
            PublishDate = adventure.PublishDate;
            InitializationScriptId = adventure.InitializationScriptId;
            TerminationScriptId = adventure.TerminationScriptId;
        }

        public Adventure ToEntity()
        {
            return new Adventure()
            {
                Id = Id,
                Name = Name,
                InitialSourceKey = InitialSourceKey,
                DescriptionSourceKey = DescriptionSourceKey,
                CreatedByUserId = CreatedByUserId,
                PublishDate = DateTime.SpecifyKind(PublishDate, DateTimeKind.Utc),
                InitializationScriptId = InitializationScriptId,
                TerminationScriptId = TerminationScriptId
            };
        }
    }
}