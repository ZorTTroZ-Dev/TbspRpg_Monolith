using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class SourcesControllerTests: ApiTest
    {
        private SourcesController CreateController(ICollection<En> sources)
        {
            var service = CreateSourcesService(sources);
            return new SourcesController(service, NullLogger<SourcesController>.Instance);
        }

        #region GetSourcesForAdventure

        [Fact]
        public async void GetSourcesForAdventure_NoKey_ReturnBadRequest()
        {
            // arrange
            var sources = new List<En>();
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourcesForAdventure(
                Guid.NewGuid(), new SourceFilterRequest()
                {
                    Key = Guid.Empty,
                    Language = Languages.ENGLISH
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetSourcesForAdventure_NoLanguage_ReturnBadRequest()
        {
            // arrange
            var sources = new List<En>();
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourcesForAdventure(
                Guid.NewGuid(), new SourceFilterRequest()
                {
                    Key = Guid.NewGuid()
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetSourcesForAdventure_Valid_ReturnSource()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourcesForAdventure(
                sources[0].AdventureId, new SourceFilterRequest()
                {
                    Key = sources[0].Key,
                    Language = Languages.ENGLISH
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(sources[0].Id, sourceViewModel.Id);
        }

        #endregion
    }
}