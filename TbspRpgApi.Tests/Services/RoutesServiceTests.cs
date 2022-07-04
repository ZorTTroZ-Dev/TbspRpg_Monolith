using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class RoutesServiceTests: ApiTest
    {
        #region GetRoutes

        [Fact]
        public async void GetRoutes_FilterLocation_ReturnRoutes()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                }
            };
            var service = CreateRoutesService(testRoutes);
            
            // act
            var routes = await service.GetRoutes(new RouteFilterRequest()
            {
                LocationId = testRoutes[0].LocationId
            });
            
            // assert
            Assert.Single(routes);
            Assert.Equal(testRoutes[0].Id, routes[0].Id);
        }

        [Fact]
        public async void GetRoutes_FilterDestination_ReturnRoutes()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                }
            };
            var service = CreateRoutesService(testRoutes);
            
            // act
            var routes = await service.GetRoutes(new RouteFilterRequest()
            {
                DestinationLocationId = testRoutes[0].DestinationLocationId
            });
            
            // assert
            Assert.Single(routes);
            Assert.Equal(testRoutes[0].Id, routes[0].Id);
        }
        
        [Fact]
        public async void GetRoutes_FilterAdventure_ReturnRoutes()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test adventure"
            };
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    Location = new Location()
                    {
                        Id = Guid.NewGuid(),
                        AdventureId = testAdventure.Id
                    }
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    Location = new Location()
                    {
                        Id = Guid.NewGuid(),
                        AdventureId = testAdventure.Id
                    }
                }
            };
            var service = CreateRoutesService(testRoutes);
            
            // act
            var routes = await service.GetRoutes(new RouteFilterRequest()
            {
                AdventureId = testAdventure.Id
            });
            
            // assert
            Assert.Equal(2, routes.Count);
            Assert.Equal(testRoutes[0].Id, routes[0].Id);
        }

        #endregion

        #region GetRouteById

        [Fact]
        public async void GetRouteById_ValidId_ReturnRoute()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                }
            };
            var service = CreateRoutesService(testRoutes);
            
            // act
            var route = await service.GetRouteById(testRoutes[1].Id);
            
            // assert
            Assert.NotNull(route);
            Assert.Equal(testRoutes[1].Id, route.Id);
        }

        [Fact]
        public async void GetRouteById_InvalidId_ReturnNull()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                }
            };
            var service = CreateRoutesService(testRoutes);
            
            // act
            var route = await service.GetRouteById(Guid.NewGuid());
            
            // assert
            Assert.Null(route);
        }

        #endregion

        #region UpdateRoutesWithSource

        [Fact]
        public async void UpdateRoutesWithSource_NoException_Returns()
        {
            // arrange
            var service = CreateRoutesService(new List<Route>(), Guid.NewGuid());
            
            // act
            await service.UpdateRoutesWithSource(new List<RouteUpdateRequest>()
            {
                new()
                {
                    route = new RouteUpdateViewModel(){
                        Id = Guid.NewGuid(),
                        Name = "route name",
                        newDestinationLocationName = ""
                    },
                    source = new SourceViewModel(Guid.Empty,"source text"),
                    successSource = new SourceViewModel(Guid.Empty,"success source text")
                }
            });

            // assert
        }
        
        [Fact]
        public async void UpdateRoutesWithSource_InValid_ExceptionThrown()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateRoutesService(new List<Route>(), exceptionId);
            
            // act
            Task Act() => service.UpdateRoutesWithSource(new List<RouteUpdateRequest>()
            {
                new()
                {
                    route = new RouteUpdateViewModel()
                    {
                        Id = exceptionId,
                        Name = "route name",
                        newDestinationLocationName = ""
                    },
                    source = new SourceViewModel(Guid.Empty,"source text"),
                    successSource = new SourceViewModel(Guid.Empty,"success source text")
                }
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion

        #region DeleteRoute

        [Fact]
        public async void DeleteRoute_InvalidRouteId_ThrowsException()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateRoutesService(new List<Route>(), exceptionId);
            
            // act
            Task Act() => service.DeleteRoute(exceptionId);
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void DeleteRoute_RouteValid_Returns()
        {
            // arrange
            var routes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    Name = "test route"
                }
            };
            var service = CreateRoutesService(routes, Guid.NewGuid());
            
            // act
            await service.DeleteRoute(routes[0].Id);

            // assert
        }

        #endregion
    }
}