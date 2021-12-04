using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IGroupsService : IBaseService
    {
        
    }
    
    public class GroupsService: IGroupsService
    {
        private readonly IGroupsRepository _groupsRepository;
        private readonly ILogger<GroupsService> _logger;
        
        public GroupsService(IGroupsRepository groupsRepository,
            ILogger<GroupsService> logger)
        {
            _groupsRepository = groupsRepository;
            _logger = logger;
        }
        
        public async Task SaveChanges()
        {
            await _groupsRepository.SaveChanges();
        }
    }
}