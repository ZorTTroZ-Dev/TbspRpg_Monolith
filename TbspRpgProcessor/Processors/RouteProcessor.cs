using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
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
        private readonly ISourceProcessor _sourceProcessor;
        private readonly IRoutesService _routesService;
        private readonly ILocationsService _locationsService;
        private readonly ILogger<RouteProcessor> _logger;

        public RouteProcessor(
            ISourceProcessor sourceProcessor,
            IRoutesService routesService,
            ILocationsService locationsService,
            ILogger<RouteProcessor> logger)
        {
            _sourceProcessor = sourceProcessor;
            _routesService = routesService;
            _locationsService = locationsService;
            _logger = logger;
        }
        
        public async Task UpdateRoute(RouteUpdateModel routeUpdateModel)
        {
            // load the location
            var dbLocation = await _locationsService.GetLocationById(routeUpdateModel.route.LocationId);
            if (dbLocation == null)
                throw new ArgumentException("route has invalid location id");
            
            // update/create route object
            Route route = null;
            if (routeUpdateModel.route.Id == Guid.Empty)
            {
                route = new Route()
                {
                    Name = routeUpdateModel.route.Name,
                    LocationId = routeUpdateModel.route.LocationId
                };
                await _routesService.AddRoute(route);
            }
            else
            {
                var dbRoute = await _routesService.GetRouteById(routeUpdateModel.route.Id);
                if (dbRoute == null)
                    throw new ArgumentException("invalid route id");
                dbRoute.Name = routeUpdateModel.route.Name;
                route = dbRoute;
            }
            
            // create update destination location set destinationLocationId
            if (!string.IsNullOrEmpty(routeUpdateModel.newDestinationLocationName))
            {
                var destinationLocation = new Location()
                {
                    Name = routeUpdateModel.newDestinationLocationName,
                    SourceKey = Guid.Empty,
                    AdventureId = dbLocation.AdventureId,
                    Initial = false,
                    Final = false
                };
                await _locationsService.AddLocation(destinationLocation);
                route.DestinationLocation = destinationLocation;
            }
            else if(routeUpdateModel.route.DestinationLocationId != Guid.Empty)
            {
                // we should make sure the destination location id is valid
                var destinationLocation = await _locationsService.GetLocationById(
                    routeUpdateModel.route.DestinationLocationId);
                if(destinationLocation != null)
                    route.DestinationLocationId = routeUpdateModel.route.DestinationLocationId;
                else
                    throw new ArgumentException("invalid destination location id");
            }
            else
            {
                // the destination location id is null, we need to throw an exception the route must have a destination
                throw new ArgumentException("empty destination location id given");
            }
            
            // create update source set sourceKey
            routeUpdateModel.source.AdventureId = dbLocation.AdventureId;
            if (string.IsNullOrEmpty(routeUpdateModel.source.Name))
                routeUpdateModel.source.Name = routeUpdateModel.route.Name;
            var dbSource = await _sourceProcessor.CreateOrUpdateSource(
                routeUpdateModel.source,
                routeUpdateModel.language);
            route.SourceKey = dbSource.Key;
            
            // create update successSource set successSourceKey
            routeUpdateModel.successSource.AdventureId = dbLocation.AdventureId;
            if (string.IsNullOrEmpty(routeUpdateModel.successSource.Name))
                routeUpdateModel.successSource.Name = routeUpdateModel.route.Name;
            var dbSuccessSource = await _sourceProcessor.CreateOrUpdateSource(
                routeUpdateModel.successSource,
                routeUpdateModel.language);
            route.RouteTakenSourceKey = dbSuccessSource.Key;

            await _routesService.SaveChanges();
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