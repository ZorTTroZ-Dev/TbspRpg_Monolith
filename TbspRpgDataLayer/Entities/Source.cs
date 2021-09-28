using System;

namespace TbspRpgApi.Entities
{
    public class Source
    {
        public Guid Id { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Guid AdventureId { get; set; }
    }
}