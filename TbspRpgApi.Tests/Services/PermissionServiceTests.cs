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

        #region CanAccessLocation

        

        #endregion
    }
}