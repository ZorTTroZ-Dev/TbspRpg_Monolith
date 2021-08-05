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
            IEnumerable<User> users = null,
            IEnumerable<Adventure> adventures = null,
            ICollection<Game> games = null,
            IEnumerable<Location> locations = null,
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
    }
}