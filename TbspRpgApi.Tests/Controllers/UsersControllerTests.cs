using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class UsersControllerTests : ApiTest
    {
        private static UsersController CreateController(IEnumerable<User> users)
        {
            return new UsersController(CreateUsersService(users),
                NullLogger<UsersController>.Instance);
        }
        
        #region Authenticate
        
        [Fact]
        public async void Authenticate_Valid_ReturnResponse()
        {
            //arrange
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "test"
            };
            var controller = CreateController(new List<User>() { testUser });
            var authRequest = new UsersAuthenticateRequest()
            {
                Email = "test",
                Password = "test"
            };
            
            //act
            var response = await controller.Authenticate(authRequest);
            
            //assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var authResponse = okObjectResult.Value as UserAuthViewModel;
            Assert.NotNull(authResponse);
            Assert.Equal("test", authResponse.Email);
            Assert.Equal(testUser.Id, authResponse.Id);
            Assert.NotNull(authResponse.Token);
        }
        
        [Fact]
        public async void Authenticate_InValid_ReturnBadResponse()
        {
            //arrange
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "test"
            };
            var controller = CreateController(new List<User>() { testUser });
            var authRequest = new UsersAuthenticateRequest()
            {
                Email = "test",
                Password = "testy"
            };
            
            //act
            var response = await controller.Authenticate(authRequest);
            
            //assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        #endregion
    }
}