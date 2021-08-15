using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface IMapsService
    {
        Task<LocationViewModel> GetCurrentLocationForGame(Guid gameId);
        Task<List<RouteViewModel>> GetCurrentRoutesForGame(Guid gameId);
    }
    
    public class MapsService : IMapsService
    {
        private readonly TbspRpgDataLayer.Services.IGamesService _gamesService;
        private readonly TbspRpgDataLayer.Services.IRoutesService _routesService;
        private readonly ILogger<MapsService> _logger;

        public MapsService(TbspRpgDataLayer.Services.IGamesService gamesService,
            TbspRpgDataLayer.Services.IRoutesService routesService,
            ILogger<MapsService> logger)
        {
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

        public async Task<List<RouteViewModel>> GetCurrentRoutesForGame(Guid gameId)
        {
            var game = await _gamesService.GetGameById(gameId);
            if (game == null || game.LocationId == Guid.Empty)
                throw new Exception("invalid game id or no location");
            var routes = await _routesService.GetRoutesForLocation(game.LocationId);
            return routes.Select(route => new RouteViewModel(route)).ToList();
        }
    }
}