using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor.Tests
{
    public class ProcessorTest
    {
        protected static IGameProcessor CreateGameProcessor(
            ICollection<User> users = null,
            ICollection<Adventure> adventures = null,
            ICollection<Game> games = null,
            ICollection<Location> locations = null,
            ICollection<Content> contents = null,
            ICollection<Script> scripts = null)
        {
            var scriptProcessor = CreateScriptProcessor(scripts);
            var usersService = MockServices.MockDataLayerUsersService(users);
            var adventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var locationsService = MockServices.MockDataLayerLocationsService(locations);
            var contentsService = MockServices.MockDataLayerContentsService(contents);
            return new GameProcessor(
                scriptProcessor,
                adventuresService,
                usersService,
                gamesService,
                locationsService,
                contentsService,
                NullLogger<GameProcessor>.Instance);
        }

        protected static ILocationProcessor CreateLocationProcessor(
            ICollection<Location> locations = null,
            ICollection<En> sources = null,
            ICollection<Route> routes = null)
        {
            var sourceProcessor = CreateSourceProcessor(sources);
            var locationService = MockServices.MockDataLayerLocationsService(locations);
            var routeService = MockServices.MockDataLayerRoutesService(routes);
            return new LocationProcessor(
                sourceProcessor,
                locationService,
                routeService,
                NullLogger<LocationProcessor>.Instance);
        }

        protected static IContentProcessor CreateContentProcessor(
            ICollection<Game> games = null,
            ICollection<En> sources = null)
        {
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var sourceProcessor = CreateSourceProcessor(sources);
            return new ContentProcessor(gamesService, sourceProcessor, NullLogger<ContentProcessor>.Instance);
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
            ICollection<En> sources = null,
            ICollection<User> users = null,
            ICollection<Game> games = null,
            ICollection<Location> locations = null,
            ICollection<Content> contents = null,
            ICollection<Route> routes = null,
            ICollection<Script> scripts = null)
        {
            var adventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var sourceProcessor = CreateSourceProcessor(sources);
            var gameProcessor = CreateGameProcessor(
                users,
                adventures,
                games,
                locations,
                contents,
                scripts);
            var locationProcessor = CreateLocationProcessor(locations, sources, routes);
            var sourceService = MockServices.MockDataLayerSourcesService(sources);
            return new AdventureProcessor(
                sourceProcessor,
                gameProcessor,
                locationProcessor,
                adventuresService,
                sourceService,
                NullLogger<AdventureProcessor>.Instance);
        }

        protected static IUserProcessor CreateUserProcessor(
            ICollection<User> users = null)
        {
            var usersService = MockServices.MockDataLayerUsersService(users);
            return new UserProcessor(
                usersService,
                MockMailClient(),
                NullLogger<UserProcessor>.Instance);
        }

        protected static IScriptProcessor CreateScriptProcessor(
            ICollection<Script> scripts = null)
        {
            var scriptsService = MockServices.MockDataLayerScriptsService(scripts);
            return new ScriptProcessor(scriptsService, NullLogger<ScriptProcessor>.Instance);
        }

        public static IMailClient MockMailClient()
        {
            var mailClient = new Mock<IMailClient>();
            mailClient.Setup(client =>
                    client.SendRegistrationVerificationMail(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string email, string registrationKey) => { });
            return mailClient.Object;
        }

        public static IUserProcessor MockUserProcessor(string exceptionEmail)
        {
            var userProcessor = new Mock<IUserProcessor>();

            userProcessor.Setup(processor =>
                    processor.RegisterUser(It.IsAny<UserRegisterModel>()))
                .ReturnsAsync((UserRegisterModel userRegisterModel) =>
                {
                    if (userRegisterModel.Email == exceptionEmail)
                        throw new ArgumentException("can't register user");
                    return new User()
                    {
                        Id = Guid.NewGuid()
                    };
                });

            userProcessor.Setup(processor =>
                    processor.VerifyUserRegistration(It.IsAny<UserVerifyRegisterModel>()))
                .ReturnsAsync((UserVerifyRegisterModel userVerifyRegisterModel) =>
                {
                    if (userVerifyRegisterModel.RegistrationKey == exceptionEmail)
                        throw new ArgumentException("can't verify registration");
                    return new User()
                    {
                        Id = Guid.NewGuid()
                    };
                });
            
            userProcessor.Setup(processor =>
                    processor.ResendUserRegistration(It.IsAny<UserRegisterResendModel>()))
                .ReturnsAsync((UserRegisterResendModel userRegisterResendModel) =>
                {
                    if (userRegisterResendModel.UserId.ToString() == exceptionEmail)
                        throw new ArgumentException("can't resend registration");
                    return new User()
                    {
                        Id = Guid.NewGuid()
                    };
                });

            return userProcessor.Object;
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
            
            adventureProcessor.Setup(service =>
                    service.RemoveAdventure(It.IsAny<AdventureRemoveModel>()))
                .Callback((AdventureRemoveModel adventureRemoveModel) =>
                {
                    if (adventureRemoveModel.AdventureId == updateAdventureExceptionId)
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
            
            gameProcessor.Setup(service =>
                    service.RemoveGame(It.IsAny<GameRemoveModel>()))
                .Callback((GameRemoveModel gameRemoveModel) =>
                {
                    if (gameRemoveModel.GameId == startGameExceptionId)
                    {
                        throw new ArgumentException("can't remove game");
                    }
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
        
        public static IScriptProcessor MockScriptProcessor(Guid executeScriptExceptionId)
        {
            var scriptProcessor = new Mock<IScriptProcessor>();
            
            scriptProcessor.Setup(service =>
                    service.ExecuteScript(It.IsAny<Guid>()))
                .Callback((Guid scriptId) =>
                {
                    if (scriptId == executeScriptExceptionId)
                    {
                        throw new ArgumentException("invalid script id");
                    }
                });
            
            return scriptProcessor.Object;
        }

        public static IContentProcessor MockContentProcessor(ICollection<Game> games, ICollection<En> sources)
        {
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var sourceProcessor = MockSourceProcessor(sources);
            return new ContentProcessor(gamesService, sourceProcessor, NullLogger<ContentProcessor>.Instance);
        }
        
        public static ISourceProcessor MockSourceProcessor(ICollection<En> sources)
        {
            var sourcesService = MockServices.MockDataLayerSourcesService(sources);
            return new SourceProcessor(sourcesService, NullLogger<SourceProcessor>.Instance);
        }
    }
}