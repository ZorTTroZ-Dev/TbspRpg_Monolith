using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Services;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor.Tests
{
    public class ProcessorTest
    {
        protected static IGameProcessor CreateGameProcessor(
            IEnumerable<User> users = null,
            IEnumerable<Adventure> adventures = null,
            ICollection<Game> games = null,
            IEnumerable<Location> locations = null,
            ICollection<Content> contents = null)
        {
            var usersService = MockServices.MockDataLayerUsersService(users);
            var adventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var locationsService = MockServices.MockDataLayerLocationsService(locations);
            var contentsService = MockServices.MockDataLayerContentsService(contents);
            return new GameProcessor(adventuresService,
                usersService,
                gamesService,
                locationsService,
                contentsService,
                NullLogger<GameProcessor>.Instance);
        }

        protected static IContentProcessor CreateContentProcessor(
            ICollection<Game> games = null,
            ICollection<En> sources = null)
        {
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var sourcesService = MockServices.MockDataLayerSourcesService(sources);
            return new ContentProcessor(gamesService, sourcesService, NullLogger<ContentProcessor>.Instance);
        }

        protected static IMapProcessor CreateMapProcessor(
            ICollection<Game> games = null,
            ICollection<Route> routes = null,
            ICollection<Content> contents = null)
        {
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var routesService = MockServices.MockDataLayerRoutesService(routes);
            var contentsService = MockServices.MockDataLayerContentsService(contents);
            return new MapProcessor(gamesService, routesService, contentsService, NullLogger<MapProcessor>.Instance);
        }

        public static IGameProcessor MockGameProcessor(Guid startGameExceptionId)
        {
            var gameProcessor = new Mock<IGameProcessor>();
            
            gameProcessor.Setup(service =>
                    service.StartGame(It.IsAny<Guid>(), It.IsAny<Guid>(),It.IsAny<DateTime>()))
                .ReturnsAsync((Guid userId, Guid adventureId, DateTime timeStamp) =>
                {
                    if (userId == startGameExceptionId)
                    {
                        throw new ArgumentException("can't start game");
                    }

                    return new Game()
                    {
                        Id = Guid.NewGuid()
                    };
                });
            
            return gameProcessor.Object;
        }
        
        public static IMapProcessor MockMapProcessor(Guid changeLocationViaRouteExceptionId)
        {
            var mapProcessor = new Mock<IMapProcessor>();
            
            mapProcessor.Setup(service =>
                    service.ChangeLocationViaRoute(It.IsAny<Guid>(), It.IsAny<Guid>(),It.IsAny<DateTime>()))
                .Callback((Guid gameId, Guid routeId, DateTime timeStamp) =>
                {
                    if (gameId == changeLocationViaRouteExceptionId)
                    {
                        throw new ArgumentException("can't start game");
                    }
                });
            
            return mapProcessor.Object;
        }

        public static IContentProcessor MockContentProcessor(ICollection<Game> games, ICollection<En> sources)
        {
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var sourcesService = MockServices.MockDataLayerSourcesService(sources);
            return new ContentProcessor(gamesService, sourcesService, NullLogger<ContentProcessor>.Instance);
        }
    }
}