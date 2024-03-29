using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services
{
    public interface IAdventuresService
    {
        Task<List<AdventureViewModel>> GetAllAdventures(AdventureFilterRequest filters);
        Task<List<AdventureViewModel>> GetPublishedAdventures(AdventureFilterRequest filters);
        Task<AdventureViewModel> GetAdventureByName(string name);
        Task<AdventureViewModel> GetAdventureById(Guid adventureId);
        Task UpdateAdventureAndSource(AdventureUpdateRequest adventureUpdateRequest, Guid userId);
        Task DeleteAdventure(Guid adventureId);
    }
    
    public class AdventuresService : IAdventuresService
    {
        private readonly TbspRpgDataLayer.Services.IAdventuresService _adventuresService;
        private readonly ITbspRpgProcessor _tbspRpgProcessor;

        public AdventuresService(
            ITbspRpgProcessor tbspRpgProcessor,
            TbspRpgDataLayer.Services.IAdventuresService adventuresService)
        {
            _tbspRpgProcessor = tbspRpgProcessor;
            _adventuresService = adventuresService;
        }

        public async Task<List<AdventureViewModel>> GetAllAdventures(AdventureFilterRequest filters)
        {
            var adventures = await _adventuresService.GetAllAdventures(
                filters.ToAdventureFilter());
            return adventures.Select(adventure => new AdventureViewModel(adventure)).ToList();
        }

        public async Task<List<AdventureViewModel>> GetPublishedAdventures(AdventureFilterRequest filters)
        {
            var adventures = await _adventuresService.GetPublishedAdventures(
                filters.ToAdventureFilter());
            return adventures.Select(adventure => new AdventureViewModel(adventure)).ToList();
        }

        public async Task<AdventureViewModel> GetAdventureByName(string name)
        {
            var adventure = await _adventuresService.GetAdventureByName(name);
            return adventure != null ? new AdventureViewModel(adventure) : null;
        }

        public async Task<AdventureViewModel> GetAdventureById(Guid adventureId)
        {
            var adventure = await _adventuresService.GetAdventureById(adventureId);
            return adventure != null ? new AdventureViewModel(adventure) : null;
        }

        public async Task UpdateAdventureAndSource(AdventureUpdateRequest adventureUpdateRequest, Guid userId)
        {
            var adventureUpdateModel = adventureUpdateRequest.ToAdventureUpdateModel();
            adventureUpdateModel.UserId = userId;
            await _tbspRpgProcessor.UpdateAdventure(adventureUpdateModel);
        }

        public async Task DeleteAdventure(Guid adventureId)
        {
            await _tbspRpgProcessor.RemoveAdventure(new AdventureRemoveModel()
            {
                AdventureId = adventureId
            });
        }
    }
}