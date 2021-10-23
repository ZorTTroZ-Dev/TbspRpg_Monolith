using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IRouteProcessor
    {
        Task UpdateRoute(RouteUpdateModel routeUpdateModel);
        Task RemoveRoutes(List<Guid> currentRouteIds, Guid locationId);
    }
    
    public class RouteProcessor: IRouteProcessor
    {
        private readonly IRoutesService _routesService;
        private readonly ILocationsService _locationsService;
        private readonly ISourcesService _sourcesService;
        private readonly ILogger<RouteProcessor> _logger;

        public RouteProcessor(IRoutesService routesService,
            ILocationsService locationsService,
            ISourcesService sourcesService,
            ILogger<RouteProcessor> logger)
        {
            _routesService = routesService;
            _locationsService = locationsService;
            _sourcesService = sourcesService;
            _logger = logger;
        }
        
        public Task UpdateRoute(RouteUpdateModel routeUpdateModel)
        {
            // load the route from the database
            // if it fails throw an exception
            // if the route id is empty create a new route
            // update/populate route name
            
            // create update source set sourceKey
            
            // create update successSource set successSourceKey
            
            // create update destination location set destinationLocationId
            throw new NotImplementedException();
        }

        public async Task RemoveRoutes(List<Guid> currentRouteIds, Guid locationId)
        {
            // get a list of routes for this location id
            // remove any routes in the db list that aren't in the given list
            var routes = await _routesService.GetRoutesForLocation(locationId);
            var dbRouteIds = routes.Select(route => route.Id);
            var idsToRemove = dbRouteIds.Except(currentRouteIds);
            routes.Where(route => idsToRemove.Contains(route.Id))
                .ToList()
                .ForEach(route => _routesService.RemoveRoute(route));
            await _routesService.SaveChanges();
        }
    }
}