using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Tests;
using TbspRpgProcessor.Processors;

namespace TbspRpgProcessor.Tests
{
    public class ProcessorTest
    {
        protected static IGameProcessor CreateGameProcessor(
            IEnumerable<User> users,
            IEnumerable<Adventure> adventures,
            IEnumerable<Game> games)
        {
            var usersService = MockServices.MockDataLayerUsersService(users);
            var adventuresService = MockServices.MockDataLayerAdventuresService(adventures);
            var gamesService = MockServices.MockDataLayerGamesService(games);
            return new GameProcessor(adventuresService,
                usersService,
                gamesService,
                NullLogger<GameProcessor>.Instance);
        }
    }
}