using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgProcessor.Processors
{
    public interface ILocationProcessor
    {
        Task UpdateLocation(Location location, Source source, string language);
    }
    
    public class LocationProcessor : ILocationProcessor
    {
        private readonly ILocationsService _locationsService;
        private readonly ISourcesService _sourcesService;
        private readonly ILogger<LocationProcessor> _logger;

        public LocationProcessor(ILocationsService locationsService,
            ISourcesService sourcesService, ILogger<LocationProcessor> logger)
        {
            _locationsService = locationsService;
            _sourcesService = sourcesService;
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
            if (source.Key == Guid.Empty)
            {
                // we need to create a new source object and save it
                var newSource = new Source()
                {
                    Key = Guid.NewGuid(),
                    AdventureId = location.AdventureId,
                    Name = location.Name,
                    Text = source.Text
                };
                await _sourcesService.AddSource(newSource);
            }
            else // existing source object
            {
                var dbSource = await _sourcesService.GetSourceForKey(source.Key, source.AdventureId, language);
                if(dbSource == null)
                    throw new ArgumentException("invalid source key");
                dbSource.Text = source.Text;
            }
            
            // save the changes
            await _locationsService.SaveChanges();
        }
    }
}