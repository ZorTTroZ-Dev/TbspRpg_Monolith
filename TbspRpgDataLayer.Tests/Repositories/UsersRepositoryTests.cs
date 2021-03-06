using System;
using System.Collections.Generic;
using System.Linq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
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
                Email = "test",
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
            Assert.Equal(testUser.Email, user.Email);
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

        [Fact]
        public async void GetUserById_Valid_GroupsAndPermissionsIncluded()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var adminPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "admin specific permission"
            };
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
                        Name = "Admin",
                        Permissions = new List<Permission>()
                        {
                            adminPermission
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Super Admin",
                        Permissions = new List<Permission>()
                        {
                            adminPermission,
                            new()
                            {
                                Id = Guid.NewGuid(),
                                Name = "super admin specific permission"
                            }
                        }
                    }
                }
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserById(testUser.Id);

            //assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.Email, user.Email);
            Assert.Equal(2, user.Groups.Count);
            Assert.Single(user.Groups.First().Permissions);
            Assert.Equal(2, user.Groups.Last().Permissions.Count);
        }

        #endregion

        #region GetUserByEmailAndPassword

        [Fact]
        public async void GetUserByEmailAndPassword_GroupsAndPermissionsIncluded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var adminPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "admin specific permission"
            };
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
                        Name = "Admin",
                        Permissions = new List<Permission>()
                        {
                            adminPermission
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Super Admin",
                        Permissions = new List<Permission>()
                        {
                            adminPermission,
                            new()
                            {
                                Id = Guid.NewGuid(),
                                Name = "super admin specific permission"
                            }
                        }
                    }
                }
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            // act
            var user = await repo.GetUserByEmailAndPassword("test", "test");
            
            // assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.Email, user.Email);
            Assert.Equal(2, user.Groups.Count);
            Assert.Single(user.Groups.First().Permissions);
            Assert.Equal(2, user.Groups.Last().Permissions.Count);
        }

        [Fact]
        public async void GetUserByEmailAndPassword_Valid_ReturnOne()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByEmailAndPassword(
                "test", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4=");

            //assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.Email, user.Email);
        }
        
        [Fact]
        public async void GetUserByEmailAndPassword_EmailCaseInsensitive_ReturnOne()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByEmailAndPassword(
                "tEST", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4=");

            //assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
            Assert.Equal(testUser.Email, user.Email);
        }
        
        [Fact]
        public async void GetUserByEmailAndPassword_InValidUsername_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByEmailAndPassword(
                "tEsTX", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4=");

            //assert
            Assert.Null(user);
        }
        
        [Fact]
        public async void GetUserByEmailAndPassword_InValidPassword_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByEmailAndPassword(
                "test", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4");
        
            //assert
            Assert.Null(user);
        }
        
        [Fact]
        public async void GetUserByEmailAndPassword_InValidBoth_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repo = new UsersRepository(context);
            
            //act
            var user = await repo.GetUserByEmailAndPassword(
                "tEStX", "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4");

            //assert
            Assert.Null(user);
        }

        #endregion

        #region GetUserByEmail

        [Fact]
        public async void GetUserByEmail_EmailExists_ReturnUser()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);
            
            // act
            var user = await repository.GetUserByEmail("test@test.com");
            
            // assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
        }

        [Fact]
        public async void GetUserByEmail_EmailExistsWrongCase_ReturnUser()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);
            
            // act
            var user = await repository.GetUserByEmail("Test@Test.com");
            
            // assert
            Assert.NotNull(user);
            Assert.Equal(testUser.Id, user.Id);
        }

        [Fact]
        public async void GetUserByEmail_EmailNotExist_ReturnNull()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            context.Users.Add(testUser);
            await context.SaveChangesAsync();
            var repository = new UsersRepository(context);
            
            // act
            var user = await repository.GetUserByEmail("testx@testx.com");
            
            // assert
            Assert.Null(user);
        }

        #endregion

        #region AddUser/SaveChanges

        [Fact]
        public async void AddUser_UserAdded()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4="
            };
            var repository = new UsersRepository(context);
            
            // act
            await repository.AddUser(testUser);
            await repository.SaveChanges();
            
            // assert
            Assert.Single(context.Users);
        }

        #endregion
    }
}