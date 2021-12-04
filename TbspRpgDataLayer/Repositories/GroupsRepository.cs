using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IGroupsRepository : IBaseRepository
    {
        
    }
    
    public class GroupsRepository: IGroupsRepository
    {
        private readonly DatabaseContext _databaseContext;
        
        public GroupsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}