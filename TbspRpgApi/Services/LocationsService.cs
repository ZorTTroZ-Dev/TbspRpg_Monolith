using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface ILocationsService
    {
        Task<List<LocationViewModel>> GetLocationsForAdventure(Guid adventureId);
    }
    
    public class LocationsService: ILocationsService
    {
        private readonly TbspRpgDataLayer.Services.ILocationsService _locationsService;
        private readonly ILogger<LocationsService> _logger;

        public LocationsService(TbspRpgDataLayer.Services.ILocationsService locationsService,
            ILogger<LocationsService> logger)
        {
            _locationsService = locationsService;
            _logger = logger;
        }
        
        public async Task<List<LocationViewModel>> GetLocationsForAdventure(Guid adventureId)
        {
            var locations = await _locationsService.GetLocationsForAdventure(adventureId);
            return locations.Select(location => new LocationViewModel(location)).ToList();
        }
    }
}