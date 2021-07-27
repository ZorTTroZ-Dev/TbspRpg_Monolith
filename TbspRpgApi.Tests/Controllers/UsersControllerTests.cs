using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    //public class UsersControllerTests : InMemoryTest
    public class UsersControllerTests
    {
        // public UsersControllerTests() : base("UsersControllerTests") { }
        //
        // private static UsersController CreateController(DatabaseContext context)
        // {
        //     var settings = new DatabaseSettings()
        //     {
        //         Salt = "y728sfLla98YUZpTgCM4VA=="
        //     };
        //     var usersService = new UsersService(
        //         new UsersRepository(context),
        //         settings,
        //         NullLogger<UsersService>.Instance);
        //     
        //     return new UsersController(usersService, NullLogger<UsersController>.Instance);
        // }
        //
        // #region Authenticate
        //
        // [Fact]
        // public async void Authenticate_Valid_ReturnResponse()
        // {
        //     //arrange
        //     await using var context = new DatabaseContext(DbContextOptions);
        //     var testUser = new User
        //     {
        //         Id = Guid.NewGuid(),
        //         UserName = "test",
        //         Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
        //     };
        //     context.Users.Add(testUser);
        //     await context.SaveChangesAsync();
        //     var controller = CreateController(context);
        //     var authRequest = new UsersAuthenticateRequest()
        //     {
        //         UserName = "test",
        //         Password = "test"
        //     };
        //     
        //     //act
        //     var response = await controller.Authenticate(authRequest);
        //     
        //     //assert
        //     var okObjectResult = response as OkObjectResult;
        //     Assert.NotNull(okObjectResult);
        //     var authResponse = okObjectResult.Value as UserViewModel;
        //     Assert.NotNull(authResponse);
        //     Assert.Equal("test", authResponse.Username);
        //     Assert.Equal(testUser.Id, authResponse.Id);
        // }
        //
        // [Fact]
        // public async void Authenticate_InValid_ReturnBadResponse()
        // {
        //     //arrange
        //     await using var context = new DatabaseContext(DbContextOptions);
        //     var testUser = new User
        //     {
        //         Id = Guid.NewGuid(),
        //         UserName = "test",
        //         Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
        //     };
        //     context.Users.Add(testUser);
        //     await context.SaveChangesAsync();
        //     var controller = CreateController(context);
        //     var authRequest = new UsersAuthenticateRequest()
        //     {
        //         UserName = "test",
        //         Password = "testy"
        //     };
        //     
        //     //act
        //     var response = await controller.Authenticate(authRequest);
        //     
        //     //assert
        //     var badRequestResult = response as BadRequestObjectResult;
        //     Assert.NotNull(badRequestResult);
        //     Assert.Equal(400, badRequestResult.StatusCode);
        // }
        //
        // #endregion
    }
}