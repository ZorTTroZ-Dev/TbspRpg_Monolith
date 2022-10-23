using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;

namespace TbspRpgApi.Services
{
    public interface ILocationsService
    {
        Task<List<LocationViewModel>> GetLocationsForAdventure(Guid adventureId);
        Task<LocationViewModel> GetLocationById(Guid locationId);
        Task UpdateLocationAndSource(LocationUpdateRequest locationUpdateRequest);
    }

    public class LocationsService : ILocationsService
    {
        private readonly TbspRpgDataLayer.Services.ILocationsService _locationsService;
        private readonly ITbspRpgProcessor _tbspRpgProcessor;
        private readonly ILogger<LocationsService> _logger;

        public LocationsService(
            ITbspRpgProcessor tbspRpgProcessor,
            TbspRpgDataLayer.Services.ILocationsService locationsService,
            ILogger<LocationsService> logger)
        {
            _tbspRpgProcessor = tbspRpgProcessor;
            _locationsService = locationsService;
            _logger = logger;
        }

        public async Task<List<LocationViewModel>> GetLocationsForAdventure(Guid adventureId)
        {
            var locations = await _locationsService.GetLocationsForAdventure(adventureId);
            return locations.Select(location => new LocationViewModel(location)).ToList();
        }

        public async Task<LocationViewModel> GetLocationById(Guid locationId)
        {
            var location = await _locationsService.GetLocationById(locationId);
            return new LocationViewModel(location);
        }

        public async Task UpdateLocationAndSource(LocationUpdateRequest locationUpdateRequest)
        {
            await _tbspRpgProcessor.UpdateLocation(locationUpdateRequest.ToLocationUpdateModel());
        }
    }
}