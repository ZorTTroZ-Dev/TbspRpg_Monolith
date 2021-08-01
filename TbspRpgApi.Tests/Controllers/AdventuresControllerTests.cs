using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.ViewModels;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class AdventuresControllerTests : ApiTest
    {
        private static AdventuresController CreateController(IEnumerable<Adventure> adventures)
        {
            var service = CreateAdventuresService(adventures);
            return new AdventuresController(service,
                NullLogger<AdventuresController>.Instance);
        }

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
            var controller = CreateController(testAdventures);
            
            // act
            var response = await controller.GetAllAdventures();
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var adventureViewModels = okObjectResult.Value as List<AdventureViewModel>;
            Assert.NotNull(adventureViewModels);
            Assert.Equal(2, adventureViewModels.Count);
        }

        #endregion
    }
}