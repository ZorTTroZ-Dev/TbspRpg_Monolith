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
            
            return permissionService.Object;
        }

        protected static PermissionService CreatePermissionService(
            IEnumerable<User> users,
            ICollection<Location> locations = null,
            ICollection<Adventure> adventures = null)
        {
            if (adventures == null)
                adventures = new List<Adventure>();
            
            var dlUsersService = MockServices.MockDataLayerUsersService(users);
            var dlLocationsService = MockServices.MockDataLayerLocationsService(locations);
            var dlAdventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            return new PermissionService(dlUsersService,
                dlLocationsService,
                dlAdventuresService,
                NullLogger<PermissionService>.Instance);
        }
        
        protected static UsersService CreateUsersService(IEnumerable<User> users)
        {
            var dlUsersService = MockServices.MockDataLayerUsersService(users);
            return new UsersService(dlUsersService,
                new JwtSettings()
                {
                    Secret = "vtqj@y31d%%j01tae3*bqu16&5$x@s@=22&bk$h9+=55kv-i6t"
                });
        }
        
        protected static AdventuresService CreateAdventuresService(ICollection<Adventure> adventures,
            Guid? updateAdventureExceptionId = null)
        {
            var dlAdventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var adventureProcessor =
                ProcessorTest.MockAdventureProcessor(updateAdventureExceptionId.GetValueOrDefault());
            return new AdventuresService(dlAdventuresService, adventureProcessor);
        }

        protected static GamesService CreateGamesService(ICollection<Game> games, Guid? startGameExceptionId = null)
        {
            startGameExceptionId ??= Guid.NewGuid();
            var dlGamesService = MockServices.MockDataLayerGamesService(games);
            var gameProcessor = ProcessorTest.MockGameProcessor(startGameExceptionId.GetValueOrDefault());
            return new GamesService(
                dlGamesService,
                gameProcessor,
                NullLogger<GamesService>.Instance);
        }

        protected static ContentsService CreateContentsService(ICollection<Content> contents,
            ICollection<Game> games = null, ICollection<En> sources = null)
        {
            var dlContentsService = MockServices.MockDataLayerContentsService(contents);
            var contentProcessor = ProcessorTest.MockContentProcessor(games, sources);
            return new ContentsService(dlContentsService, 
                contentProcessor, NullLogger<ContentsService>.Instance);
        }

        protected static MapsService CreateMapsService(ICollection<Game> games,
            ICollection<Route> routes = null,
            Guid? changeLocationViaRouteExceptionId = null)
        {
            var dlGamesService = MockServices.MockDataLayerGamesService(games);
            var dlRoutesService = MockServices.MockDataLayerRoutesService(routes);
            var mapProcessor = ProcessorTest.MockMapProcessor(changeLocationViaRouteExceptionId.GetValueOrDefault());
            return new MapsService(
                dlGamesService,
                dlRoutesService,
                mapProcessor,
                NullLogger<MapsService>.Instance);
        }

        protected static LocationsService CreateLocationsService(ICollection<Location> locations,
            Guid? updateLocationExceptionId = null)
        {
            var dlLocationsService = MockServices.MockDataLayerLocationsService(locations);
            var locationProcessor = ProcessorTest.MockLocationProcessor(updateLocationExceptionId.GetValueOrDefault());
            return new LocationsService(
                dlLocationsService,
                locationProcessor,
                NullLogger<LocationsService>.Instance);
        }

        protected static SourcesService CreateSourcesService(ICollection<En> sources)
        {
            var dlSourcesService = MockServices.MockDataLayerSourcesService(sources);
            return new SourcesService(
                dlSourcesService,
                NullLogger<SourcesService>.Instance);
        }

        protected static RoutesService CreateRoutesService(
            ICollection<Route> routes = null,
            Guid? updateRouteExceptionId = null)
        {
            var routeProcessor = ProcessorTest.MockRouteProcessor(updateRouteExceptionId.GetValueOrDefault());
            var dlRoutesService = MockServices.MockDataLayerRoutesService(routes);
            return new RoutesService(
                routeProcessor,
                dlRoutesService,
                NullLogger<RoutesService>.Instance);
        }
    }
}