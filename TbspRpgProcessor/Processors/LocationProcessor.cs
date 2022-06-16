using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface ILocationProcessor
    {
        Task UpdateLocation(Location location, Source source, string language);
        Task RemoveLocation(LocationRemoveModel locationRemoveModel);
        Task RemoveLocation(Location location, bool save = true);
        Task RemoveLocations(ICollection<Location> locations, bool save = true);
    }
    
    public class LocationProcessor : ILocationProcessor
    {
        private readonly ISourceProcessor _sourceProcessor;
        private readonly ILocationsService _locationsService;
        private readonly IRoutesService _routesService;
        private readonly ILogger<LocationProcessor> _logger;

        public LocationProcessor(
            ISourceProcessor sourceProcessor,
            ILocationsService locationsService,
            IRoutesService routesService,
            ILogger<LocationProcessor> logger)
        {
            _sourceProcessor = sourceProcessor;
            _locationsService = locationsService;
            _routesService = routesService;
            _logger = logger;
        }
        
        public async Task UpdateLocation(Location location, Source source, string language)
        {
            Location dbLocation = null;
            if (location.Id == Guid.Empty)
            {
                dbLocation = new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = location.Name,
                    Initial = location.Initial,
                    Final = location.Final,
                    AdventureId = location.AdventureId,
                    EnterScriptId = location.EnterScriptId,
                    ExitScriptId = location.ExitScriptId
                };
                await _locationsService.AddLocation(dbLocation);
            }
            else
            {
                // load the location, update the name and initial fields, save it
                dbLocation = await _locationsService.GetLocationById(location.Id);
                if (dbLocation == null)
                    throw new ArgumentException("invalid location id");
                dbLocation.Name = location.Name;
                dbLocation.Initial = location.Initial;
                dbLocation.Final = location.Final;
                dbLocation.EnterScriptId = location.EnterScriptId;
                dbLocation.ExitScriptId = location.ExitScriptId;
            }
            
            // update the source
            source.AdventureId = location.AdventureId;
            if (string.IsNullOrEmpty(source.Name))
                source.Name = location.Name;
            var dbSource = await _sourceProcessor.CreateOrUpdateSource(source, language);
            dbLocation.SourceKey = dbSource.Key;
            
            // save the changes
            await _locationsService.SaveChanges();
        }

        public async Task RemoveLocation(LocationRemoveModel locationRemoveModel)
        {
            var dbLocation = await _locationsService.GetLocationByIdWithRoutes(locationRemoveModel.LocationId);
            if (dbLocation == null)
            {
                throw new ArgumentException("invalid location id");
            }

            await RemoveLocation(dbLocation);
        }

        public async Task RemoveLocation(Location location, bool save = true)
        {
            if (location.Routes != null && location.Routes.Count > 0)
            {
                _routesService.RemoveRoutes(location.Routes);
            }
            _locationsService.RemoveLocation(location);
            
            if(save)
                await _locationsService.SaveChanges();
        }

        public async Task RemoveLocations(ICollection<Location> locations, bool save = true)
        {
            foreach (var location in locations)
            {
                await RemoveLocation(location, false);
            }

            if (save)
                await _locationsService.SaveChanges();
        }
    }
}