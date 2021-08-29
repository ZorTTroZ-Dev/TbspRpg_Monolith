using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgProcessor.Processors
{
    public interface IMapProcessor
    {
        Task ChangeLocationViaRoute(Guid gameId, Guid routeId, DateTime timeStamp);
    }
    
    public class MapProcessor: IMapProcessor
    {
        private readonly IGamesService _gamesService;
        private readonly IRoutesService _routesService;
        private readonly IContentsService _contentsService;
        private readonly ILogger<MapProcessor> _logger;

        public MapProcessor(IGamesService gamesService,
            IRoutesService routesService,
            IContentsService contentsService,
            ILogger<MapProcessor> logger)
        {
            _gamesService = gamesService;
            _routesService = routesService;
            _contentsService = contentsService;
            _logger = logger;
        }
        
        // make sure the game exists, throw exception
        // make sure the route exists, throw exception
        // make sure the game is in the correct location, throw exception
        // check if the player can take route
        //  if fail add failure content to game
        //  if pass update location id, location time stamp, add pass content to game
        public async Task ChangeLocationViaRoute(Guid gameId, Guid routeId, DateTime timeStamp)
        {
            var game = await _gamesService.GetGameById(gameId);
            if (game == null)
            {
                throw new ArgumentException("invalid game id");
            }

            var route = await _routesService.GetRouteById(routeId);
            if (route == null)
            {
                throw new ArgumentException("invalid route id");
            }

            if (game.LocationId != route.LocationId)
            {
                throw new Exception("game not in location it should be");
            }
            
            // for now assume the check passed
            var secondsSinceEpoch = new DateTimeOffset(timeStamp).ToUnixTimeMilliseconds();
            game.LocationId = route.DestinationLocationId;
            game.LocationUpdateTimeStamp = secondsSinceEpoch;
            
            // create content entry for the initial location source key
            await _contentsService.AddContent(new Content()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                Position = (ulong)secondsSinceEpoch,
                SourceKey = route.SuccessSourceKey
            });

            // save context changes
            await _gamesService.SaveChanges();
        }
    }
}