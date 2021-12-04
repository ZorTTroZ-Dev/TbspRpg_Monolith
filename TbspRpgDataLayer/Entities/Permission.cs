using System;
using System.Collections.Generic;

namespace TbspRpgDataLayer.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}