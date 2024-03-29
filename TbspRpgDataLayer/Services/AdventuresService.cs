using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<Adventure> GetAdventureByIdIncludeAssociatedObjects(Guid adventureId);
        Task AddAdventure(Adventure adventure);
        void RemoveAdventure(Adventure adventure);
        Task RemoveScriptFromAdventures(Guid scriptId);
        Task<Adventure> GetAdventureWithSource(Guid adventureId, Guid sourceKey);
        Task<bool> DoesAdventureUseSource(Guid adventureId, Guid sourceKey);
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

        public Task<Adventure> GetAdventureByIdIncludeAssociatedObjects(Guid adventureId)
        {
            return _adventuresRepository.GetAdventureByIdIncludeAssociatedObjects(adventureId);
        }

        public async Task AddAdventure(Adventure adventure)
        {
            await _adventuresRepository.AddAdventure(adventure);
        }

        public void RemoveAdventure(Adventure adventure)
        {
            _adventuresRepository.RemoveAdventure(adventure);
        }

        public async Task RemoveScriptFromAdventures(Guid scriptId)
        {
            var adventures = await _adventuresRepository.GetAdventuresWithScript(scriptId);
            foreach (var adventure in adventures)
            {
                if (adventure.InitializationScriptId == scriptId)
                    adventure.InitializationScriptId = null;
                if (adventure.TerminationScriptId == scriptId)
                    adventure.TerminationScriptId = null;
            }
        }

        public Task<Adventure> GetAdventureWithSource(Guid adventureId, Guid sourceKey)
        {
            return _adventuresRepository.GetAdventureWithSource(adventureId, sourceKey);
        }

        public async Task<bool> DoesAdventureUseSource(Guid adventureId, Guid sourceKey)
        {
            var adventure = await GetAdventureWithSource(adventureId, sourceKey);
            return adventure != null;
        }

        public async Task SaveChanges()
        {
            await _adventuresRepository.SaveChanges();
        }
    }
}