using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;
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
            
            return gameProcessor.Object;
        }
    }
}