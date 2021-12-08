﻿using System;
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
                    UserName = "admin",
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
                    UserName = "admin",
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
                    UserName = "admin",
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
                    UserName = "admin",
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

        #region CanAccessGame

        

        #endregion

        #region CanAccessAdventure

        

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
                    UserName = "admin",
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
                    UserName = "admin",
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
                                    Name = TbspRpgSettings.Settings.Permissions.READ_LOCATION
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
                    UserName = "admin",
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
                    UserName = "admin",
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
                    UserName = "admin",
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
                    UserName = "admin",
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
                                    Name = TbspRpgSettings.Settings.Permissions.WRITE_LOCATION
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
                    UserName = "admin",
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
                    UserName = "admin",
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