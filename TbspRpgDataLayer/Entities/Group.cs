using System;
using System.Collections.Generic;

namespace TbspRpgDataLayer.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}