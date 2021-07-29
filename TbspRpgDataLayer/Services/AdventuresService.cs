using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IAdventuresService
    {
        Task<List<Adventure>> GetAllAdventures();
    }
    
    public class AdventuresService : IAdventuresService
    {
        private readonly IAdventuresRepository _adventuresRepository;
        private readonly ILogger<AdventuresService> _logger;

        public AdventuresService(IAdventuresRepository adventuresRepository,
            ILogger<AdventuresService> logger)
        {
            _adventuresRepository = adventuresRepository;
            _logger = logger;
        }

        public Task<List<Adventure>> GetAllAdventures()
        {
            return _adventuresRepository.GetAllAdventures();
        }
    }
}