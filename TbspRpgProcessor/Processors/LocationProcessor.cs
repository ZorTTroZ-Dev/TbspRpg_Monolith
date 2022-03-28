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
        Task RemoveLocation(Location location);
        Task RemoveLocations(ICollection<Location> locations);
    }
    
    public class LocationProcessor : ILocationProcessor
    {
        private readonly ISourceProcessor _sourceProcessor;
        private readonly ILocationsService _locationsService;
        private readonly ILogger<LocationProcessor> _logger;

        public LocationProcessor(
            ISourceProcessor sourceProcessor,
            ILocationsService locationsService,
            ILogger<LocationProcessor> logger)
        {
            _sourceProcessor = sourceProcessor;
            _locationsService = locationsService;
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
                    AdventureId = location.AdventureId
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

        public Task RemoveLocation(LocationRemoveModel locationRemoveModel)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLocations(ICollection<Location> locations)
        {
            throw new NotImplementedException();
        }
    }
}