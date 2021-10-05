using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Adapters;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services
{
    public interface ILocationsService
    {
        Task<List<LocationViewModel>> GetLocationsForAdventure(Guid adventureId);
        Task UpdateLocationAndSource(UpdateLocationRequest updateRequest);
    }

    public class LocationsService : ILocationsService
    {
        private readonly TbspRpgDataLayer.Services.ILocationsService _locationsService;
        private readonly ILocationProcessor _locationProcessor;
        private readonly ILogger<LocationsService> _logger;

        public LocationsService(TbspRpgDataLayer.Services.ILocationsService locationsService,
            ILocationProcessor locationProcessor,
            ILogger<LocationsService> logger)
        {
            _locationsService = locationsService;
            _locationProcessor = locationProcessor;
            _logger = logger;
        }

        public async Task<List<LocationViewModel>> GetLocationsForAdventure(Guid adventureId)
        {
            var locations = await _locationsService.GetLocationsForAdventure(adventureId);
            return locations.Select(location => new LocationViewModel(location)).ToList();
        }

        public async Task UpdateLocationAndSource(UpdateLocationRequest updateRequest)
        {
            var location = LocationAdapter.ToEntity(updateRequest.location);
            var source = SourceAdapter.ToEntity(updateRequest.source);
            await _locationProcessor.UpdateLocation(location, source, updateRequest.source.Language);
        }
    }
}