using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Entities;

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
            ICollection<Content> contents = null)
        {
            users ??= new List<User>();
            adventures ??= new List<Adventure>();
            routes ??= new List<Route>();
            locations ??= new List<Location>();
            sources ??= new List<En>();
            games ??= new List<Game>();
            contents ??= new List<Content>();
            scripts ??= new List<Script>();
            
            var usersService = MockServices.MockDataLayerUsersService(users);
            var scriptsService = MockServices.MockDataLayerScriptsService(scripts);
            var adventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var routesService = MockServices.MockDataLayerRoutesService(routes);
            var locationsService = MockServices.MockDataLayerLocationsService(locations);
            var sourcesService = MockServices.MockDataLayerSourcesService(sources);
            var gamesService = MockServices.MockDataLayerGamesService(games);
            var contentsService = MockServices.MockDataLayerContentsService(contents);
            
            return new TbspRpgProcessor(
                usersService,
                sourcesService,
                scriptsService,
                adventuresService,
                routesService,
                locationsService,
                gamesService,
                contentsService,
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
                    service.ExecuteScript(It.IsAny<Guid>()))
                .Callback((Guid scriptId) =>
                {
                    if (scriptId == exceptionId)
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
                    service.RemoveRoutes(It.IsAny <List<Guid>>(), It.IsAny<Guid>()))
                .Callback((List<Guid> routeIds, Guid locationId) => { });
            
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
                    service.ChangeLocationViaRoute(It.IsAny<Guid>(), It.IsAny<Guid>(),It.IsAny<DateTime>()))
                .Callback((Guid gameId, Guid routeId, DateTime timeStamp) =>
                {
                    if (gameId == exceptionId)
                    {
                        throw new ArgumentException("can't change location");
                    }
                });
            
            tbspProcessor.Setup(service =>
                    service.UpdateLocation(It.IsAny<Location>(), It.IsAny<Source>(), It.IsAny<string>()))
                .Callback((Location location, Source source, string language) =>
                {
                    if (location.Id == exceptionId)
                    {
                        throw new ArgumentException("can't update location");
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

            return tbspProcessor.Object;
        }
    }
}