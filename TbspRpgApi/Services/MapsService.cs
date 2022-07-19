using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;

namespace TbspRpgApi.Services
{
    public interface IMapsService
    {
        Task<LocationViewModel> GetCurrentLocationForGame(Guid gameId);
        Task<RouteListViewModel> GetCurrentRoutesForGame(Guid gameId);
        Task<RouteListViewModel> GetCurrentRoutesForGameAfterTimeStamp(Guid gameId, long timeStamp);
        Task ChangeLocationViaRoute(Guid gameId, Guid routeId, DateTime timeStamp);
    }
    
    public class MapsService : IMapsService
    {
        private readonly TbspRpgDataLayer.Services.IGamesService _gamesService;
        private readonly TbspRpgDataLayer.Services.IRoutesService _routesService;
        private readonly ITbspRpgProcessor _tbspRpgProcessor;
        private readonly ILogger<MapsService> _logger;

        public MapsService(
            ITbspRpgProcessor tbspRpgProcessor,
            TbspRpgDataLayer.Services.IGamesService gamesService,
            TbspRpgDataLayer.Services.IRoutesService routesService,
            ILogger<MapsService> logger)
        {
            _tbspRpgProcessor = tbspRpgProcessor;
            _gamesService = gamesService;
            _routesService = routesService;
            _logger = logger;
        }
        
        public async Task<LocationViewModel> GetCurrentLocationForGame(Guid gameId)
        {
            var game = await _gamesService.GetGameByIdIncludeLocation(gameId);
            if (game?.Location == null)
                throw new Exception("invalid game id or no location");
            return new LocationViewModel(game.Location);
        }

        // If these prove to be useful, move them in to the data layer service
        // and make them single queries
        public async Task<RouteListViewModel> GetCurrentRoutesForGame(Guid gameId)
        {
            var game = await _gamesService.GetGameByIdIncludeLocation(gameId);
            if (game == null || game.LocationId == Guid.Empty)
                throw new Exception("invalid game id or no location");
            var routes = await _routesService.GetRoutesForLocation(game.LocationId);
            return new RouteListViewModel()
            {
                Location = new LocationViewModel(game.Location),
                Routes = routes.Select(route => new RouteViewModel(route, game)).ToList()
            };
        }
        
        public async Task<RouteListViewModel> GetCurrentRoutesForGameAfterTimeStamp(Guid gameId, long timeStamp)
        {
            var game = await _gamesService.GetGameByIdIncludeLocation(gameId);
            if (game == null || game.LocationId == Guid.Empty)
                throw new Exception("invalid game id or no location");
            if (game.LocationUpdateTimeStamp <= timeStamp)
                return new RouteListViewModel()
                {
                    Location = new LocationViewModel(game.Location),
                    Routes = new List<RouteViewModel>()
                };
            var routes = await _routesService.GetRoutesForLocation(game.LocationId);
            return new RouteListViewModel()
            {
                Location = new LocationViewModel(game.Location),
                Routes = routes.Select(route => new RouteViewModel(route, game)).ToList()
            };
        }

        public async Task ChangeLocationViaRoute(Guid gameId, Guid routeId, DateTime timeStamp)
        {
            await _tbspRpgProcessor.ChangeLocationViaRoute(gameId, routeId, timeStamp);
        }
    }
}