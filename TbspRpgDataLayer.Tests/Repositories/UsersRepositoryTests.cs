using System;
using TbspRpgApi.Entities;
using TbspRpgDataLayer;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgApi.Tests.Repositories
{
    public class UsersRepositoryTests : InMemoryTest
    {
        public UsersRepositoryTests() : base("UsersRepositoryTests") { }

        #region GetUserById

        [Fact]
        public async void GetUserById_Valid_ReturnOne()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "test",
                Password = "test"
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserById(testUser.Id);

            //assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.UserName, user.UserName);
        }
        
        [Fact]
        public async void GetUserById_InValid_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserById(Guid.NewGuid());

            //assert
            Assert.Null(user);
        }

        #endregion

        #region GetUserByUserNameAndPassword

        [Fact]
        public async void GetUserByUsernameAndPassword_Valid_ReturnOne()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByUsernameAndPassword(
                "test", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4=");

            //assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.UserName, user.UserName);
        }
        
        [Fact]
        public async void GetUserByUsernameAndPassword_InValidUsername_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByUsernameAndPassword(
                "tEsT", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4=");

            //assert
            Assert.Null(user);
        }
        
        [Fact]
        public async void GetUserByUsernameAndPassword_InValidPassword_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByUsernameAndPassword(
                "test", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4");
        
            //assert
            Assert.Null(user);
        }
        
        [Fact]
        public async void GetUserByUsernameAndPassword_InValidBoth_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByUsernameAndPassword(
                "tESt", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4");

            //assert
            Assert.Null(user);
        }

        #endregion
    }
}