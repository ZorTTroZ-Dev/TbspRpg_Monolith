using System;

namespace TbspRpgApi.Entities
{
    public class Content
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public ulong Position { get; set; }
        public Guid SourceId { get; set; }
        
        public Game Game { get; set; }
    }
}