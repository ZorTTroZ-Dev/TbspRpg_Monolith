using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Tests;

namespace TbspRpgApi.Tests
{
    public class ApiTest
    {
        protected static UsersService CreateUsersService(IEnumerable<User> users)
        {
            var dlUsersService = MockServices.MockDataLayerUsersService(users);
            return new UsersService(dlUsersService,
                new JwtSettings()
                {
                    Secret = "vtqj@y31d%%j01tae3*bqu16&5$x@s@=22&bk$h9+=55kv-i6t"
                });
        }
        
        protected static AdventuresService CreateAdventuresService(IEnumerable<Adventure> adventures)
        {
            var dlAdventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            return new AdventuresService(dlAdventuresService);
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

        protected static LocationsService CreateLocationsService(ICollection<Location> locations)
        {
            var dlLocationsService = MockServices.MockDataLayerLocationsService(locations);
            return new LocationsService(
                dlLocationsService,
                NullLogger<LocationsService>.Instance);
        }
    }
}