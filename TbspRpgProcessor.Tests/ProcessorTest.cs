using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Entities;
using TbspRpgSettings;

namespace TbspRpgProcessor.Tests
{
    public class ProcessorTest
    {
        public static ITbspRpgProcessor CreateTbspRpgProcessor(
            ICollection<User> users = null,
            ICollection<Script> scripts = null,
            ICollection<Adventure> adventures = null,
            ICollection<Route> routes = null,
            ICollection<Location> locations = null,
            ICollection<En> sources = null,
            ICollection<Game> games = null,
            ICollection<Content> contents = null,
            ICollection<AdventureObject> adventureObjects = null)
        {
            users ??= new List<User>();
            adventures ??= new List<Adventure>();
            routes ??= new List<Route>();
            locations ??= new List<Location>();
            sources ??= new List<En>();
            games ??= new List<Game>();
            contents ??= new List<Content>();
            scripts ??= new List<Script>();
            adventureObjects ??= new List<AdventureObject>();
            
            var usersService = MockServices.MockDataLayerUsersService(users);
            var scriptsService = MockServices.MockDataLayerScriptsService(scripts);
            var adventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var routesService = MockServices.MockDataLayerRoutesService(routes);
            var locationsService = MockServices.MockDataLayerLocationsService(locations);
            var sourcesService = MockServices.MockDataLayerSourcesService(sources);
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var contentsService = MockServices.MockDataLayerContentsService(contents);
            var adventureObjectsService = MockServices.MockDataLayerAdventureObjectsService(adventureObjects);
            var adventureObjectSourceService =
                MockServices.MockDataLayerAdventureObjectsSourceService(adventureObjects, sources);
            
            return new TbspRpgProcessor(
                usersService,
                sourcesService,
                scriptsService,
                adventuresService,
                routesService,
                locationsService,
                gamesService,
                contentsService,
                adventureObjectsService,
                adventureObjectSourceService,
                new TbspRpgUtilities(),
                MockMailClient(),
                NullLogger<TbspRpgProcessor>.Instance);
        }

        public static IMailClient MockMailClient()
        {
            var mailClient = new Mock<IMailClient>();
            mailClient.Setup(client =>
                    client.SendRegistrationVerificationMail(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string email, string registrationKey) => { });
            return mailClient.Object;
        }

        public static ITbspRpgProcessor MockTbspRpgProcessor(string exceptionEmail, Guid exceptionId)
        {
            var tbspProcessor = new Mock<ITbspRpgProcessor>();

            tbspProcessor.Setup(processor =>
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

            tbspProcessor.Setup(processor =>
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
            
            tbspProcessor.Setup(processor =>
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
            
            tbspProcessor.Setup(service =>
                    service.ExecuteScript(It.IsAny<ScriptExecuteModel>()))
                .Callback((ScriptExecuteModel scriptExecuteModel) =>
                {
                    if (scriptExecuteModel.ScriptId == exceptionId)
                    {
                        throw new ArgumentException("invalid script id");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.UpdateScript(It.IsAny<ScriptUpdateModel>()))
                .Callback((ScriptUpdateModel scriptUpdateModel) =>
                {
                    if (scriptUpdateModel.script.Id == exceptionId)
                    {
                        throw new ArgumentException("invalid script id");
                    }
                });

            tbspProcessor.Setup(service =>
                    service.RemoveScript(It.IsAny<ScriptRemoveModel>()))
                .Callback((ScriptRemoveModel scriptRemoveModel) =>
                {
                    if (scriptRemoveModel.ScriptId == exceptionId)
                    {
                        throw new ArgumentException("invalid script id");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.UpdateRoute(It.IsAny<RouteUpdateModel>()))
                .Callback((RouteUpdateModel routeUpdateModel) =>
                {
                    if (routeUpdateModel.route.Id == exceptionId)
                        throw new ArgumentException("can't update route");
                });

            tbspProcessor.Setup(service =>
                    service.RemoveRoutes(It.IsAny <RoutesRemoveModel>()))
                .Callback((RoutesRemoveModel routesRemoveModel) => { });
            
            tbspProcessor.Setup(service =>
                    service.RemoveRoute(It.IsAny<RouteRemoveModel>()))
                .Callback((RouteRemoveModel routeRemoveModel) =>
                {
                    if (routeRemoveModel.RouteId == exceptionId)
                    {
                        throw new ArgumentException("invalid route id");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.ChangeLocationViaRoute(It.IsAny<MapChangeLocationModel>()))
                .Callback((MapChangeLocationModel mapChangeLocationModel) =>
                {
                    if (mapChangeLocationModel.GameId == exceptionId)
                    {
                        throw new ArgumentException("can't change location");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.UpdateLocation(It.IsAny<LocationUpdateModel>()))
                .Callback((LocationUpdateModel locationUpdateModel) =>
                {
                    if (locationUpdateModel.Location.Id == exceptionId)
                    {
                        throw new ArgumentException("can't update location");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.RemoveLocation(It.IsAny<LocationRemoveModel>()))
                .Callback((LocationRemoveModel locationRemoveModel) =>
                {
                    if (locationRemoveModel.LocationId == exceptionId)
                    {
                        throw new ArgumentException("invalid location id");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.StartGame(It.IsAny<GameStartModel>()))
                .ReturnsAsync((GameStartModel gameStartModel) =>
                {
                    if (gameStartModel.UserId == exceptionId)
                    {
                        throw new ArgumentException("can't start game");
                    }

                    return new Game()
                    {
                        Id = Guid.NewGuid()
                    };
                });
            
            tbspProcessor.Setup(service =>
                    service.RemoveGame(It.IsAny<GameRemoveModel>()))
                .Callback((GameRemoveModel gameRemoveModel) =>
                {
                    if (gameRemoveModel.GameId == exceptionId)
                    {
                        throw new ArgumentException("can't remove game");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.UpdateAdventure(It.IsAny<AdventureUpdateModel>()))
                .Callback((AdventureUpdateModel adventureUpdateModel) =>
                {
                    if (adventureUpdateModel.Adventure.Id == exceptionId)
                        throw new ArgumentException("invalid adventure id");
                });
            
            tbspProcessor.Setup(service =>
                    service.RemoveAdventure(It.IsAny<AdventureRemoveModel>()))
                .Callback((AdventureRemoveModel adventureRemoveModel) =>
                {
                    if (adventureRemoveModel.AdventureId == exceptionId)
                        throw new ArgumentException("invalid adventure id");
                });
            
            tbspProcessor.Setup(service =>
                    service.RemoveAdventureObject(It.IsAny<AdventureObjectRemoveModel>()))
                .Callback((AdventureObjectRemoveModel adventureRemoveModel) =>
                {
                    if (adventureRemoveModel.AdventureObjectId == exceptionId)
                        throw new ArgumentException("invalid adventure object id");
                });
            
            tbspProcessor.Setup(service =>
                    service.UpdateAdventureObject(It.IsAny<AdventureObjectUpdateModel>()))
                .Callback((AdventureObjectUpdateModel adventureObjectUpdateModel) =>
                {
                    if (adventureObjectUpdateModel.AdventureObject.Id == exceptionId)
                    {
                        throw new ArgumentException("invalid adventure object id");
                    }
                });

            return tbspProcessor.Object;
        }
    }
}