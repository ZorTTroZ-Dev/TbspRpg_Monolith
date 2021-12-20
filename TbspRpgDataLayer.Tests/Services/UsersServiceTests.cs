using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class UsersServiceTests : InMemoryTest
    {
        public UsersServiceTests() : base("UsersServiceTests") { }

        private static UsersService CreateService(DatabaseContext context)
        {
            var settings = new DatabaseSettings()
            {
                Salt = "y728sfLla98YUZpTgCM4VA=="
            };
            return new UsersService(
                new UsersRepository(context),
                settings,
                NullLogger<UsersService>.Instance);
        }

        #region GetUserById

        [Fact]
        public async void GetUserById_Valid_ReturnUser()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var user = await service.GetById(testUser.Id);
            
            // assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.Email, user.Email);
        }
        
        [Fact]
        public async void GetUserById_InValid_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var user = await service.GetById(Guid.NewGuid());
            
            // assert
            Assert.Null(user);
        }
        
        [Fact]
        public async void GetUserById_Valid_GroupsIncluded()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "test",
                Groups = new List<Group>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Super Admin"
                    }
                }
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var user = await service.GetById(testUser.Id);

            //assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.Email, user.Email);
            Assert.Equal(2, testUser.Groups.Count);
        }

        #endregion

        #region GetUserByEmailAndPassword

        [Fact]
        public async void Authenticate_Valid_ReturnUser()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var user = await service.Authenticate("test", "test");
            
            // assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.Email, user.Email);
        }
        
        [Fact]
        public async void Authenticate_InValidPassword_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var user = await service.Authenticate("test", "testt");
            
            // assert
            Assert.Null(user);
        }
        
        [Fact]
        public async void Authenticate_InValidUserName_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var user = await service.Authenticate("testy", "test");
            
            // assert
            Assert.Null(user);
        }

        #endregion
    }
}