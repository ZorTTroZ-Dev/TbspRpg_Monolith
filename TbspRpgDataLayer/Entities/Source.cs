using System;

namespace TbspRpgDataLayer.Entities
{
    public class Source
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Guid AdventureId { get; set; }
        public Guid ScriptId { get; set; }
        
        public Script Script { get; set; }
    }
}