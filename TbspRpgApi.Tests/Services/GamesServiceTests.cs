using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class GamesServiceTests : ApiTest
    {
        #region StartGame
        
        // these tests are kind of lame.
        [Fact]
        public async void StartGame_ExceptionThrown()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateGamesService(new List<Game>(), exceptionId);
            
            // act
            Task Act() => service.StartGame(exceptionId, Guid.NewGuid(), DateTime.Now);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void StartGame_GameStarted()
        {
            // arrange
            var service = CreateGamesService(new List<Game>(), Guid.NewGuid());
            
            // act
            await service.StartGame(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            
            // assert
            
        }

        #endregion
    }
}