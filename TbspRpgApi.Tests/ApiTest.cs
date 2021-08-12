using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

        protected static ContentsService CreateContentsService(ICollection<Content> contents)
        {
            var dlContentsService = MockServices.MockDataLayerContentsService(contents);
            return new ContentsService(dlContentsService, NullLogger<ContentsService>.Instance);
        }
    }
}