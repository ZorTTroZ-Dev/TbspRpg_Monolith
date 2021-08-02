using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;
using TbspRpgApi.ViewModels;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class UsersControllerTests : ApiTest
    {
        private static UsersController CreateController(IEnumerable<User> users)
        {
            var dlUsersService = MockDataLayerUsersService(users);
            return new UsersController(new UsersService(dlUsersService),
                new JwtSettings()
                {
                    Secret = "vtqj@y31d%%j01tae3*bqu16&5$x@s@=22&bk$h9+=55kv-i6t"
                },
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
                UserName = "test",
                Password = "test"
            };
            var controller = CreateController(new List<User>() { testUser });
            var authRequest = new UsersAuthenticateRequest()
            {
                UserName = "test",
                Password = "test"
            };
            
            //act
            var response = await controller.Authenticate(authRequest);
            
            //assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var authResponse = okObjectResult.Value as UserAuthViewModel;
            Assert.NotNull(authResponse);
            Assert.Equal("test", authResponse.Username);
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
                UserName = "test",
                Password = "test"
            };
            var controller = CreateController(new List<User>() { testUser });
            var authRequest = new UsersAuthenticateRequest()
            {
                UserName = "test",
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