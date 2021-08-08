using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
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

        protected static GamesService CreateGamesService(ICollection<Game> games, Guid startGameExceptionId)
        {
            var dlGamesService = MockServices.MockDataLayerGamesService(games);
            var gameProcessor = ProcessorTest.MockGameProcessor(startGameExceptionId);
            return new GamesService(
                dlGamesService,
                gameProcessor,
                NullLogger<GamesService>.Instance);
        }
    }
}