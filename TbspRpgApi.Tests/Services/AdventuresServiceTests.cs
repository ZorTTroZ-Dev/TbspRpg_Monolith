using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
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
                    InitialSourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test two",
                    InitialSourceKey = Guid.NewGuid()
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
                InitialSourceKey = Guid.NewGuid()
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
                InitialSourceKey = Guid.NewGuid()
            };
            var service = CreateAdventuresService(new List<Adventure>() {testAdventure});
            
            // act`
            var adventureViewModel = await service.GetAdventureByName("testy");
            
            // assert
            Assert.Null(adventureViewModel);
        }

        #endregion
        
        #region GetAdventureById

        [Fact]
        public async void GetAdventureById_Exists_ReturnAdventure()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                InitialSourceKey = Guid.NewGuid()
            };
            var service = CreateAdventuresService(new List<Adventure>() {testAdventure});
            
            // act`
            var adventureViewModel = await service.GetAdventureById(testAdventure.Id);
            
            // assert
            Assert.NotNull(adventureViewModel);
            Assert.Equal(testAdventure.Id, adventureViewModel.Id);
        }

        [Fact]
        public async void GetAdventureById_NotExist_ReturnNull()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                InitialSourceKey = Guid.NewGuid()
            };
            var service = CreateAdventuresService(new List<Adventure>() {testAdventure});
            
            // act`
            var adventureViewModel = await service.GetAdventureById(Guid.NewGuid());
            
            // assert
            Assert.Null(adventureViewModel);
        }

        #endregion

        #region UpdateAdventureAndSource

        [Fact]
        public async void UpdateAdventureAndSource_Invalid_ExceptionThrown()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateAdventuresService(new List<Adventure>(), exceptionId);
            
            // act
            Task Act() => service.UpdateAdventureAndSource(new AdventureUpdateRequest()
            {
                adventure = new AdventureViewModel(new Adventure()
                {
                    Id = exceptionId,
                    InitialSourceKey = Guid.NewGuid(),
                    Name = "test location"
                }),
                initialSource = new SourceViewModel(Guid.Empty, "test source"),
                descriptionSource = new SourceViewModel(Guid.Empty, "test description source")
            }, Guid.NewGuid());
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void UpdateAdventureAndSource_Valid_AdventureUpdated()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateAdventuresService(new List<Adventure>(), exceptionId);
            
            // act
            await service.UpdateAdventureAndSource(new AdventureUpdateRequest()
            {
                adventure = new AdventureViewModel(new Adventure()
                {
                    Id = Guid.NewGuid(),
                    InitialSourceKey = Guid.NewGuid(),
                    Name = "test location"
                }),
                initialSource = new SourceViewModel(Guid.Empty, "test source"),
                descriptionSource = new SourceViewModel(Guid.Empty, "test description source")
            }, Guid.NewGuid());
            
        }

        #endregion
    }
}