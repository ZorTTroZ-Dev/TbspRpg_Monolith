using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class AdventuresControllerTests : ApiTest
    {
        private static AdventuresController CreateController(ICollection<Adventure> adventures, Guid? exceptionId = null)
        {
            var service = CreateAdventuresService(adventures, exceptionId);
            var controller = new AdventuresController(service,
                NullLogger<AdventuresController>.Instance)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            
            controller.ControllerContext.HttpContext.Items = new Dictionary<object, object>()
            {
                { AuthorizeAttribute.USER_ID_CONTEXT_KEY, Guid.NewGuid() }
            };
            return controller;
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
                    InitialSourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test two",
                    InitialSourceKey = Guid.NewGuid()
                }
            };
            var controller = CreateController(testAdventures);
            
            // act
            var response = await controller.GetAllAdventures(new AdventureFilterRequest());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var adventureViewModels = okObjectResult.Value as List<AdventureViewModel>;
            Assert.NotNull(adventureViewModels);
            Assert.Equal(2, adventureViewModels.Count);
        }

        #endregion

        #region GetAdventureByName

        [Fact]
        public async void GetAdventureByName_Exists_ReturnAdventure()
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
            var controller = CreateController(testAdventures);
            
            // act
            var response = await controller.GetAdventureByName("test");
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var adventureViewModel = okObjectResult.Value as AdventureViewModel;
            Assert.NotNull(adventureViewModel);
            Assert.Equal(testAdventures[0].Id, adventureViewModel.Id);
        }
        
        [Fact]
        public async void GetAdventureByName_NotExist_ReturnNull()
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
            var controller = CreateController(testAdventures);
            
            // act
            var response = await controller.GetAdventureByName("testy");
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var adventureViewModel = okObjectResult.Value as AdventureViewModel;
            Assert.Null(adventureViewModel);
            //Assert.Equal(testAdventures[0].Id, adventureViewModel.Id);
        }

        #endregion
        
        #region GetAdventureById

        [Fact]
        public async void GetAdventureById_Exists_ReturnAdventure()
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
            var controller = CreateController(testAdventures);
            
            // act
            var response = await controller.GetAdventureById(testAdventures[0].Id);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var adventureViewModel = okObjectResult.Value as AdventureViewModel;
            Assert.NotNull(adventureViewModel);
            Assert.Equal(testAdventures[0].Id, adventureViewModel.Id);
        }
        
        [Fact]
        public async void GetAdventureById_NotExist_ReturnNull()
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
            var controller = CreateController(testAdventures);
            
            // act
            var response = await controller.GetAdventureById(Guid.NewGuid());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var adventureViewModel = okObjectResult.Value as AdventureViewModel;
            Assert.Null(adventureViewModel);
            //Assert.Equal(testAdventures[0].Id, adventureViewModel.Id);
        }

        #endregion

        #region UpdateAdventureAndSource
        
        [Fact]
        public async void UpdateLocationAndSource_Valid_ReturnOk()
        {
            // arrange
            var controller = CreateController(new List<Adventure>(), Guid.NewGuid());
            
            // act
            var response = await controller.UpdateAdventureAndSource(
                new AdventureUpdateRequest()
                {
                    adventure = new AdventureViewModel(new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        InitialSourceKey = Guid.NewGuid(),
                        Name = "test location"
                    }),
                    initialSource = new SourceViewModel(Guid.Empty, "test source"),
                    descriptionSource = new SourceViewModel(Guid.Empty, "test description source")
                });

            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        public async void UpdateLocationAndSource_UpdateFails_ReturnBadRequest()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateController(new List<Adventure>(), exceptionId);
            
            // act
            var response = await controller.UpdateAdventureAndSource(
                new AdventureUpdateRequest()
                {
                    adventure = new AdventureViewModel(new Adventure()
                    {
                        Id = exceptionId,
                        InitialSourceKey = Guid.NewGuid(),
                        Name = "test location"
                    }),
                    initialSource = new SourceViewModel(Guid.Empty, "test source"),
                    descriptionSource = new SourceViewModel(Guid.Empty, "test description source")
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion
    }
}