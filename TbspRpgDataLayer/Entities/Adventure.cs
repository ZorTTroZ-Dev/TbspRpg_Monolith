using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;

namespace TbspRpgDataLayer.Entities
{
    public class Adventure
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid InitialSourceKey { get; set; }
        public Guid DescriptionSourceKey { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime PublishDate { get; set; }
        public Guid? InitializationScriptId { get; set; }
        public Guid? TerminationScriptId { get; set; }
        
        public ICollection<Location> Locations { get; set; }
        public ICollection<Game> Games { get; set; }
        public User CreatedByUser { get; set; }
        public Script InitializationScript { get; set; }
        public Script TerminationScript { get; set; }
    }
}