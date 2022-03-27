using System;
using System.Collections.Generic;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class PermissionServiceTests: ApiTest
    {
        #region HasPermission

        [Fact]
        public async void HasPermission_HasPermission_ReturnTrue()
        {
            // arrange
            var commonPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "admin permission"
            };
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission,
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "other admin permission"
                                }
                            }
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "user_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission
                            }
                        }
                    }
                }
            };
            var service = CreatePermissionService(users);
            
            // act
            var hasPermission = await service.HasPermission(
                users[0].Id, "other admin permission");
            
            // assert
            Assert.True(hasPermission);
        }

        [Fact]
        public async void HasPermission_DoesntHavePermission_ReturnFalse()
        {
            // arrange
            var commonPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "admin permission"
            };
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission,
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "other admin permission"
                                }
                            }
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "user_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission
                            }
                        }
                    }
                }
            };
            var service = CreatePermissionService(users);
            
            // act
            var hasPermission = await service.HasPermission(
                users[0].Id, "bananas");
            
            // assert
            Assert.False(hasPermission);
        }

        #endregion

        #region IsInGroup

        [Fact]
        public async void IsInGroup_IsInGroup_ReturnTrue()
        {
            // arrange
            var commonPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "admin permission"
            };
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission,
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "other admin permission"
                                }
                            }
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "user_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission
                            }
                        }
                    }
                }
            };
            var service = CreatePermissionService(users);
            
            // act
            var isInGroup = await service.IsInGroup(
                users[0].Id, "admin_group");
            
            // await
            Assert.True(isInGroup);
        }

        [Fact]
        public async void IsInGroup_IsntInGroup_ReturnFalse()
        {
            // arrange
            var commonPermission = new Permission()
            {
                Id = Guid.NewGuid(),
                Name = "admin permission"
            };
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission,
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "other admin permission"
                                }
                            }
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "user_group",
                            Permissions = new List<Permission>()
                            {
                                commonPermission
                            }
                        }
                    }
                }
            };
            var service = CreatePermissionService(users);
            
            // act
            var isInGroup = await service.IsInGroup(
                users[0].Id, "bananas");
            
            // await
            Assert.False(isInGroup);
        }

        #endregion

        #region CanReadGame

        [Fact]
        public async void CanReadGame_OwnsGame_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = users[0].Id
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanReadGame(users[0].Id, games[0].Id);
            
            // assert
            Assert.True(can);
        }
        
        [Fact]
        public async void CanReadGame_HasPermission_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = TbspRpgSettings.Settings.Permissions.ReadGame
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanReadGame(users[0].Id, games[0].Id);
            
            // assert
            Assert.True(can);
        }
        
        [Fact]
        public async void CanReadGame_NoOwnNoPermission_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanReadGame(users[0].Id, games[0].Id);
            
            // assert
            Assert.False(can);
        }
        
        [Fact]
        public async void CanReadGame_BadGameId_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = users[0].Id
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanReadGame(users[0].Id, Guid.NewGuid());
            
            // assert
            Assert.False(can);
        }

        #endregion

        #region CanWriteGame

        [Fact]
        public async void CanWriteGame_OwnsGame_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = users[0].Id
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanWriteGame(users[0].Id, games[0].Id);
            
            // assert
            Assert.True(can);
        }
        
        [Fact]
        public async void CanWriteGame_HasPermission_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = TbspRpgSettings.Settings.Permissions.WriteGame
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanWriteGame(users[0].Id, games[0].Id);
            
            // assert
            Assert.True(can);
        }
        
        [Fact]
        public async void CanWriteGame_NoOwnNoPermission_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanWriteGame(users[0].Id, games[0].Id);
            
            // assert
            Assert.False(can);
        }
        
        [Fact]
        public async void CanWriteGame_BadGameIdPermissionOverride_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = TbspRpgSettings.Settings.Permissions.WriteGame
                                }
                            }
                        }
                    }
                }
            };
            var games = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = users[0].Id
                }
            };
            var service = CreatePermissionService(users, null, null, games);
            
            // act
            var can = await service.CanWriteGame(users[0].Id, Guid.NewGuid());
            
            // assert
            Assert.True(can);
        }

        #endregion

        #region CanReadAdventure

        [Fact]
        public async void CanReadAdventure_OwnAdventure_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = users[0].Id
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canRead = await service.CanReadAdventure(users[0].Id, adventures[0].Id);
            
            // assert
            Assert.True(canRead);
        }

        [Fact]
        public async void CanReadAdventure_HasPermission_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = TbspRpgSettings.Settings.Permissions.ReadAdventure
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canRead = await service.CanReadAdventure(users[0].Id, adventures[0].Id);
            
            // assert
            Assert.True(canRead);
        }

        [Fact]
        public async void CanReadAdventure_NoOwnNoPermission_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "bananas"
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canRead = await service.CanReadAdventure(users[0].Id, adventures[0].Id);
            
            // assert
            Assert.False(canRead);
        }

        [Fact]
        public async void CanReadAdventure_BadAdventureId_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "bananas"
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canRead = await service.CanReadAdventure(users[0].Id, Guid.NewGuid());
            
            // assert
            Assert.False(canRead);
        }

        #endregion
        
        #region CanWriteAdventure

        [Fact]
        public async void CanWriteAdventure_OwnAdventure_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = users[0].Id
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canWrite = await service.CanWriteAdventure(users[0].Id, adventures[0].Id);
            
            // assert
            Assert.True(canWrite);
        }
        
        [Fact]
        public async void CanWriteAdventure_HasPermission_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = TbspRpgSettings.Settings.Permissions.WriteAdventure
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canWrite = await service.CanWriteAdventure(users[0].Id, adventures[0].Id);
            
            // assert
            Assert.True(canWrite);
        }
        
        [Fact]
        public async void CanWriteAdventure_NoOwnerNoPermission_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "bananas"
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = Guid.NewGuid()
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canWrite = await service.CanWriteAdventure(users[0].Id, adventures[0].Id);
            
            // assert
            Assert.False(canWrite);
        }
        
        [Fact]
        public async void CanWriteAdventure_BadAdventureId_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "bananas"
                                }
                            }
                        }
                    }
                }
            };
            var adventures = new List<Adventure>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    CreatedByUserId = users[0].Id
                }
            };
            var service = CreatePermissionService(users, null, adventures);
            
            // act
            var canWrite = await service.CanWriteAdventure(users[0].Id, Guid.NewGuid());
            
            // assert
            Assert.False(canWrite);
        }

        #endregion

        #region CanReadLocation

        [Fact]
        public async void CanReadLocation_OwnsLocation_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = users[0].Id
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canRead = await service.CanReadLocation(users[0].Id, locations[0].Id);
            
            // assert
            Assert.True(canRead);
        }

        [Fact]
        public async void CanReadLocation_HasPermission_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = TbspRpgSettings.Settings.Permissions.ReadLocation
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = Guid.NewGuid()
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canRead = await service.CanReadLocation(users[0].Id, locations[0].Id);
            
            // assert
            Assert.True(canRead);
        }

        [Fact]
        public async void CanReadLocation_NotOwnerNoPermission_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = Guid.NewGuid()
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canRead = await service.CanReadLocation(users[0].Id, locations[0].Id);
            
            // assert
            Assert.False(canRead);
        }

        [Fact]
        public async void CanReadLocation_BadLocationId_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = users[0].Id
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canRead = await service.CanReadLocation(users[0].Id, Guid.NewGuid());
            
            // assert
            Assert.False(canRead);
        }

        #endregion
        
        #region CanWriteLocation

        [Fact]
        public async void CanWriteLocation_OwnsLocation_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "bananas"
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = users[0].Id
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canWrite = await service.CanWriteLocation(users[0].Id, locations[0].Id);
            
            // assert
            Assert.True(canWrite);
        }

        [Fact]
        public async void CanWriteLocation_HasPermission_ReturnTrue()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = TbspRpgSettings.Settings.Permissions.WriteLocation
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = Guid.NewGuid()
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canWrite = await service.CanWriteLocation(users[0].Id, locations[0].Id);
            
            // assert
            Assert.True(canWrite);
        }

        [Fact]
        public async void CanWriteLocation_NotOwnerNoPermission_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = Guid.NewGuid()
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canWrite = await service.CanWriteLocation(users[0].Id, locations[0].Id);
            
            // assert
            Assert.False(canWrite);
        }

        [Fact]
        public async void CanWriteLocation_BadLocationId_ReturnFalse()
        {
            // arrange
            var users = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin",
                    Groups = new List<Group>()
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            Name = "admin_group",
                            Permissions = new List<Permission>()
                            {
                                new()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "banana"
                                }
                            }
                        }
                    }
                }
            };
            var locations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test location",
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test adventure",
                        CreatedByUserId = users[0].Id
                    }
                }
            };
            var service = CreatePermissionService(
                users, locations);
            
            // act
            var canWrite = await service.CanWriteLocation(users[0].Id, Guid.NewGuid());
            
            // assert
            Assert.False(canWrite);
        }

        #endregion
    }
}