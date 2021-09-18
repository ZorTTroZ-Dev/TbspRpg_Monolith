using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface ILocationsService
    {
        Task<Location> GetInitialLocationForAdventure(Guid adventureId);
        Task<List<Location>> GetLocationsForAdventure(Guid adventureId);
    }
    
    public class LocationsService : ILocationsService
    {
        private readonly ILocationsRepository _locationsRepository;
        private readonly ILogger<LocationsService> _logger;

        public LocationsService(
            ILocationsRepository locationsRepository,
            ILogger<LocationsService> logger)
        {
            _locationsRepository = locationsRepository;
            _logger = logger;
        }

        public Task<Location> GetInitialLocationForAdventure(Guid adventureId)
        {
            return _locationsRepository.GetInitialForAdventure(adventureId);
        }

        public Task<List<Location>> GetLocationsForAdventure(Guid adventureId)
        {
            return _locationsRepository.GetLocationsForAdventure(adventureId);
        }
    }
}