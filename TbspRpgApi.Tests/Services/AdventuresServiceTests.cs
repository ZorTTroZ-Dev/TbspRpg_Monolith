using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class AdventuresServiceTests : ApiTest
    {
        #region GetAllAdventures

        [Fact]
        public async void GetAllAdventures_ReturnsAllAdventures()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test two",
                    SourceKey = Guid.NewGuid()
                }
            };
            var service = CreateAdventuresService(testAdventures);
            
            // act
            var adventures = await service.GetAllAdventures(new AdventureFilterRequest());
            
            // assert
            Assert.Equal(2, adventures.Count);
            Assert.Equal(testAdventures[0].Id, adventures[0].Id);
        }

        #endregion

        #region GetAdventureByName

        [Fact]
        public async void GetAdventureByName_Exists_ReturnAdventure()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                SourceKey = Guid.NewGuid()
            };
            var service = CreateAdventuresService(new List<Adventure>() {testAdventure});
            
            // act`
            var adventureViewModel = await service.GetAdventureByName("test");
            
            // assert
            Assert.NotNull(adventureViewModel);
            Assert.Equal(testAdventure.Id, adventureViewModel.Id);
        }

        [Fact]
        public async void GetAdventureByName_NotExist_ReturnNull()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                SourceKey = Guid.NewGuid()
            };
            var service = CreateAdventuresService(new List<Adventure>() {testAdventure});
            
            // act`
            var adventureViewModel = await service.GetAdventureByName("testy");
            
            // assert
            Assert.Null(adventureViewModel);
        }

        #endregion
    }
}