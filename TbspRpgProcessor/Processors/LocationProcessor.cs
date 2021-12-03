using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgProcessor.Processors
{
    public interface ILocationProcessor
    {
        Task UpdateLocation(Location location, Source source, string language);
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
            // load the location, update the name and initial fields, save it
            var dbLocation = await _locationsService.GetLocationById(location.Id);
            if(dbLocation == null)
                throw new ArgumentException("invalid location id");
            dbLocation.Name = location.Name;
            dbLocation.Initial = location.Initial;

            // update the source
            source.AdventureId = location.AdventureId;
            if (string.IsNullOrEmpty(source.Name))
                source.Name = location.Name;
            var dbSource = await _sourceProcessor.CreateOrUpdateSource(source, language);
            dbLocation.SourceKey = dbSource.Key;
            
            // save the changes
            await _locationsService.SaveChanges();
        }
    }
}