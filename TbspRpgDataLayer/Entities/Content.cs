using System;
using TbspRpgApi.Entities;

namespace TbspRpgDataLayer.Entities
{
    public class Content
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public ulong Position { get; set; }
        public Guid SourceKey { get; set; }
        
        public Game Game { get; set; }
    }
}