using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface IAdventuresService
    {
        Task<List<AdventureViewModel>> GetAllAdventures();
        Task<AdventureViewModel> GetAdventureByName(string name);
    }
    
    public class AdventuresService : IAdventuresService
    {
        private readonly TbspRpgDataLayer.Services.IAdventuresService _adventuresService;

        public AdventuresService(TbspRpgDataLayer.Services.IAdventuresService adventuresService)
        {
            _adventuresService = adventuresService;
        }

        public async Task<List<AdventureViewModel>> GetAllAdventures()
        {
            var adventures = await _adventuresService.GetAllAdventures();
            return adventures.Select(adventure => new AdventureViewModel(adventure)).ToList();
        }

        public async Task<AdventureViewModel> GetAdventureByName(string name)
        {
            var adventure = await _adventuresService.GetAdventureByName(name);
            return adventure != null ? new AdventureViewModel(adventure) : null;
        }
    }
}