using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgApi.Services;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class UsersServiceTests : ApiTest
    {
        #region Authenticate

        [Fact]
        public async void Authenticate_Valid_ReturnViewModel()
        {
            //arrange
            var usersService = CreateUsersService(new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserName = "test",
                    Password = "test"
                }
            });
            
            //act
            var user = await usersService.Authenticate("test", "test");
            
            //assert
            Assert.NotNull(user);
            Assert.Equal("test", user.Username);
        }
        
        [Fact]
        public async void Authenticate_InValid_ReturnNull()
        {
            //arrange
            var usersService = CreateUsersService(new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserName = "test",
                    Password = "test"
                }
            });
            
            //act
            var user = await usersService.Authenticate("testy", "test");
            
            //assert
            Assert.Null(user);
        }

        #endregion

        #region GetUserById

        [Fact]
        public async void GetUserById_Valid_ReturnsUserWithGroups()
        {
            
        }

        #endregion
    }
}