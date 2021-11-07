using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Services;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor.Tests
{
    public class ProcessorTest
    {
        protected static IGameProcessor CreateGameProcessor(
            IEnumerable<User> users = null,
            ICollection<Adventure> adventures = null,
            ICollection<Game> games = null,
            ICollection<Location> locations = null,
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

        protected static ILocationProcessor CreateLocationProcessor(
            ICollection<Location> locations = null,
            ICollection<En> sources = null)
        {
            var sourceProcessor = CreateSourceProcessor(sources);
            var locationService = MockServices.MockDataLayerLocationsService(locations);
            return new LocationProcessor(
                sourceProcessor,
                locationService,
                NullLogger<LocationProcessor>.Instance);
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

        protected static ISourceProcessor CreateSourceProcessor(
            ICollection<En> sources = null)
        {
            var sourcesService = MockServices.MockDataLayerSourcesService(sources);
            return new SourceProcessor(sourcesService, NullLogger<SourceProcessor>.Instance);
        }

        protected static IRouteProcessor CreateRouteProcessor(
            ICollection<Route> routes = null,
            ICollection<Location> locations = null,
            ICollection<En> sources = null)
        {
            var routesService = MockServices.MockDataLayerRoutesService(routes);
            var sourceProcessor = CreateSourceProcessor(sources);
            var locationService = MockServices.MockDataLayerLocationsService(locations);
            return new RouteProcessor(
                sourceProcessor,
                routesService,
                locationService,
                NullLogger<RouteProcessor>.Instance);
        }

        protected static IAdventureProcessor CreateAdventureProcessor(
            ICollection<Adventure> adventures = null,
            ICollection<En> sources = null)
        {
            var adventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var sourceProcessor = CreateSourceProcessor(sources);
            return new AdventureProcessor(
                sourceProcessor,
                adventuresService,
                NullLogger<AdventureProcessor>.Instance);
        }

        public static IAdventureProcessor MockAdventureProcessor(Guid updateAdventureExceptionId)
        {
            var adventureProcessor = new Mock<IAdventureProcessor>();

            adventureProcessor.Setup(service =>
                    service.UpdateAdventure(It.IsAny<AdventureUpdateModel>()))
                .Callback((AdventureUpdateModel adventureUpdateModel) =>
                {
                    if (adventureUpdateModel.Adventure.Id == updateAdventureExceptionId)
                        throw new ArgumentException("invalid adventure id");
                });

            return adventureProcessor.Object;
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

        public static ILocationProcessor MockLocationProcessor(Guid updateLocationExceptionId)
        {
            var locationProcessor = new Mock<ILocationProcessor>();

            locationProcessor.Setup(service =>
                    service.UpdateLocation(It.IsAny<Location>(), It.IsAny<Source>(), It.IsAny<string>()))
                .Callback((Location location, Source source, string language) =>
                {
                    if (location.Id == updateLocationExceptionId)
                    {
                        throw new ArgumentException("can't update location");
                    }
                });
            return locationProcessor.Object;
        }

        public static IRouteProcessor MockRouteProcessor(Guid updateRouteExceptionId)
        {
            var routeProcessor = new Mock<IRouteProcessor>();

            routeProcessor.Setup(service =>
                    service.UpdateRoute(It.IsAny<RouteUpdateModel>()))
                .Callback((RouteUpdateModel routeUpdateModel) =>
                {
                    if (routeUpdateModel.route.Id == updateRouteExceptionId)
                        throw new ArgumentException("can't update route");
                });

            routeProcessor.Setup(service =>
                    service.RemoveRoutes(It.IsAny <List<Guid>>(), It.IsAny<Guid>()))
                .Callback((List<Guid> routeIds, Guid locationId) => { });

            return routeProcessor.Object;
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
                        throw new ArgumentException("can't change location");
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