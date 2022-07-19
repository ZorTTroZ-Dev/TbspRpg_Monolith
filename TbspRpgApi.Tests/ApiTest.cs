using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Tests;

namespace TbspRpgApi.Tests
{
    public class ApiTest
    {
        protected static IPermissionService MockPermissionService()
        {
            // return true for all permissions
            var permissionService = new Mock<IPermissionService>();
            
            permissionService.Setup(service =>
                    service.HasPermission(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync((Guid userId, string permissionName) => true);
            
            permissionService.Setup(service =>
                    service.IsInGroup(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync((Guid userId, string permissionName) => true);
            
            permissionService.Setup(service =>
                    service.CanReadLocation(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid locationId) => true);
            
            permissionService.Setup(service =>
                    service.CanWriteLocation(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid locationId) => true);
            
            permissionService.Setup(service =>
                    service.CanReadAdventure(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid locationId) => true);
            
            permissionService.Setup(service =>
                    service.CanWriteAdventure(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid locationId) => true);
            
            permissionService.Setup(service =>
                    service.CanReadGame(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid locationId) => true);
            
            permissionService.Setup(service =>
                    service.CanWriteGame(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid locationId) => true);
            
            permissionService.Setup(service =>
                    service.CanDeleteGame(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid gameId) => true);

            permissionService.Setup(service =>
                    service.CanDeleteScript(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid scriptId) => true);
            
            permissionService.Setup(service =>
                    service.CanDeleteRoute(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Guid userId, Guid routeId) => true);
            
            return permissionService.Object;
        }

        protected static PermissionService CreatePermissionService(
            ICollection<User> users,
            ICollection<Location> locations = null,
            ICollection<Adventure> adventures = null,
            ICollection<Game> games = null,
            ICollection<Script> scripts = null,
            ICollection<Route> routes = null)
        {
            locations ??= new List<Location>();
            adventures ??= new List<Adventure>();
            games ??= new List<Game>();
            routes ??= new List<Route>();
            
            var dlUsersService = MockServices.MockDataLayerUsersService(users);
            var dlLocationsService = MockServices.MockDataLayerLocationsService(locations);
            var dlAdventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var dlGamesService = MockServices.MockDataLayerGamesService(games);
            var dlScriptsService = MockServices.MockDataLayerScriptsService(scripts);
            var dlRoutesService = MockServices.MockDataLayerRoutesService(routes);
            return new PermissionService(dlUsersService,
                dlLocationsService,
                dlAdventuresService,
                dlGamesService,
                dlScriptsService,
                dlRoutesService,
                NullLogger<PermissionService>.Instance);
        }
        
        protected static UsersService CreateUsersService(
            ICollection<User> users,
            string exceptionEmail = null)
        {
            var dlUsersService = MockServices.MockDataLayerUsersService(users);
            var mockTbspRpgProcessor = ProcessorTest.MockTbspRpgProcessor(exceptionEmail, Guid.Empty);
            return new UsersService(dlUsersService,
                mockTbspRpgProcessor,
                new JwtSettings()
                {
                    Secret = "vtqj@y31d%%j01tae3*bqu16&5$x@s@=22&bk$h9+=55kv-i6t"
                });
        }
        
        protected static AdventuresService CreateAdventuresService(ICollection<Adventure> adventures,
            Guid? updateAdventureExceptionId = null)
        {
            var dlAdventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var tbspRpgProcessor = ProcessorTest.MockTbspRpgProcessor(null,
                updateAdventureExceptionId.GetValueOrDefault());
            return new AdventuresService(tbspRpgProcessor, dlAdventuresService);
        }

        protected static GamesService CreateGamesService(ICollection<Game> games, Guid? startGameExceptionId = null)
        {
            startGameExceptionId ??= Guid.NewGuid();
            var dlGamesService = MockServices.MockDataLayerGamesService(games);
            var tbspRpgProcessor = ProcessorTest.MockTbspRpgProcessor(null,
                startGameExceptionId.GetValueOrDefault());
            return new GamesService(
                tbspRpgProcessor,
                dlGamesService,
                NullLogger<GamesService>.Instance);
        }

        protected static ContentsService CreateContentsService(ICollection<Content> contents,
            Guid scriptExceptionId, ICollection<Game> games = null, ICollection<En> sources = null)
        {
            var dlContentsService = MockServices.MockDataLayerContentsService(contents);
            var tbspRpgProcessor = ProcessorTest.CreateTbspRpgProcessor(
                null, null, null, null, null,
                sources, games);
            return new ContentsService(
                tbspRpgProcessor,
                dlContentsService, 
                NullLogger<ContentsService>.Instance);
        }

        protected static MapsService CreateMapsService(ICollection<Game> games,
            ICollection<Route> routes = null,
            Guid? changeLocationViaRouteExceptionId = null)
        {
            var dlGamesService = MockServices.MockDataLayerGamesService(games);
            var dlRoutesService = MockServices.MockDataLayerRoutesService(routes);
            var tbspRpgProcessor =
                ProcessorTest.MockTbspRpgProcessor(null, changeLocationViaRouteExceptionId.GetValueOrDefault());
            return new MapsService(
                tbspRpgProcessor,
                dlGamesService,
                dlRoutesService,
                NullLogger<MapsService>.Instance);
        }

        protected static LocationsService CreateLocationsService(ICollection<Location> locations,
            Guid? updateLocationExceptionId = null)
        {
            var dlLocationsService = MockServices.MockDataLayerLocationsService(locations);
            var tbspRpgProcessor =
                ProcessorTest.MockTbspRpgProcessor(null, updateLocationExceptionId.GetValueOrDefault());
            return new LocationsService(
                tbspRpgProcessor,
                dlLocationsService,
                NullLogger<LocationsService>.Instance);
        }

        protected static SourcesService CreateSourcesService(ICollection<En> sources, Guid scriptExceptionId)
        {
            var dlSourcesService = MockServices.MockDataLayerSourcesService(sources);
            var tbspRpgProcessor = ProcessorTest.CreateTbspRpgProcessor(
                null, null, null, null, null,
                sources);
            return new SourcesService(
                dlSourcesService,
                tbspRpgProcessor,
                NullLogger<SourcesService>.Instance);
        }

        protected static RoutesService CreateRoutesService(
            ICollection<Route> routes = null,
            Guid? updateRouteExceptionId = null)
        {
            var mockTbspRpgProcessor = ProcessorTest.MockTbspRpgProcessor(null, updateRouteExceptionId.GetValueOrDefault());
            var dlRoutesService = MockServices.MockDataLayerRoutesService(routes);
            return new RoutesService(
                mockTbspRpgProcessor,
                dlRoutesService,
                NullLogger<RoutesService>.Instance);
        }

        protected static ScriptsService CreateScriptsService(
            ICollection<Script> scripts = null,
            Guid? exceptionId = null)
        {
            var mockTbspRpgProcessor = ProcessorTest.MockTbspRpgProcessor(null, exceptionId.GetValueOrDefault());
            var dlScriptService = MockServices.MockDataLayerScriptsService(scripts);
            return new ScriptsService(
                mockTbspRpgProcessor,
                dlScriptService,
                NullLogger<ScriptsService>.Instance);
        }
    }
}