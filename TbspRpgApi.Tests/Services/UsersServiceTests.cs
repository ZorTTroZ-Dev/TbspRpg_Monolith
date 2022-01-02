using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;
using TbspRpgApi.ViewModels;
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
                    Email = "test",
                    Password = "test"
                }
            });
            
            //act
            var user = await usersService.Authenticate("test", "test");
            
            //assert
            Assert.NotNull(user);
            Assert.Equal("test", user.Email);
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
                    Email = "test",
                    Password = "test"
                }
            });
            
            //act
            var user = await usersService.Authenticate("testy", "test");
            
            //assert
            Assert.Null(user);
        }

        #endregion

        #region Register

        [Fact]
        public async void Register_Valid_ReturnUserViewModel()
        {
            // arrange
            var usersService = CreateUsersService(new List<User>());
            
            // act
            var user = await usersService.Register(new UsersRegisterRequest()
            {
                Email = "test@test.com",
                Password = "test"
            });
            
            // assert
            Assert.NotNull(user);
        }

        [Fact]
        public async void Register_Invalid_ExceptionThrown()
        {
            // arrange
            var usersService = CreateUsersService(new List<User>(), "test@test.com");
            
            // act
            Task Act() => usersService.Register(new UsersRegisterRequest()
            {
                Email = "test@test.com",
                Password = "test"
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion

        #region VerifyRegistration

        [Fact]
        public async void VerifyRegistration_Valid_ReturnUserViewModel()
        {
            // arrange
            var usersService = CreateUsersService(new List<User>());
            
            // act
            var user = await usersService.VerifyRegistration(new UsersRegisterVerifyRequest()
            {
                UserId = Guid.NewGuid(),
                RegistrationKey = "000000"
            });
            
            // assert
            Assert.NotNull(user);
        }

        [Fact]
        public async void VerifyRegistration_Invalid_ExceptionThrown()
        {
            // arrange
            var usersService = CreateUsersService(new List<User>(), "000000");
            
            // act
            Task Act() => usersService.VerifyRegistration(new UsersRegisterVerifyRequest()
            {
                UserId = Guid.NewGuid(),
                RegistrationKey = "000000"
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion
        
        #region RegisterResend

        [Fact]
        public async void RegisterResend_Valid_ReturnUserViewModel()
        {
            // arrange
            var usersService = CreateUsersService(new List<User>());
            
            // act
            var user = await usersService.RegisterResend(new UsersRegisterResendRequest()
            {
                UserId = Guid.NewGuid()
            });
            
            // assert
            Assert.NotNull(user);
        }

        [Fact]
        public async void RegisterResend_Invalid_ExceptionThrown()
        {
            // arrange
            var exceptionGuid = Guid.NewGuid();
            var usersService = CreateUsersService(new List<User>(), exceptionGuid.ToString());
            
            // act
            Task Act() => usersService.RegisterResend(new UsersRegisterResendRequest()
            {
                UserId = exceptionGuid
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion
    }
}