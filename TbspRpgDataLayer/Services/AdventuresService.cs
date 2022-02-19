using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IAdventuresService: IBaseService
    {
        Task<List<Adventure>> GetAllAdventures(AdventureFilter filters);
        Task<List<Adventure>> GetPublishedAdventures(AdventureFilter filters);
        Task<Adventure> GetAdventureByName(string name);
        Task<Adventure> GetAdventureById(Guid adventureId);
        Task AddAdventure(Adventure adventure);
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

        public Task<List<Adventure>> GetAllAdventures(AdventureFilter filters)
        {
            return _adventuresRepository.GetAllAdventures(filters);
        }

        public Task<List<Adventure>> GetPublishedAdventures(AdventureFilter filters)
        {
            return _adventuresRepository.GetPublishedAdventures(filters);
        }

        public Task<Adventure> GetAdventureByName(string name)
        {
            return _adventuresRepository.GetAdventureByName(name);
        }

        public Task<Adventure> GetAdventureById(Guid adventureId)
        {
            return _adventuresRepository.GetAdventureById(adventureId);
        }

        public async Task AddAdventure(Adventure adventure)
        {
            await _adventuresRepository.AddAdventure(adventure);
        }

        public async Task SaveChanges()
        {
            await _adventuresRepository.SaveChanges();
        }
    }
}