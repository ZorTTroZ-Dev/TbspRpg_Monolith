using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
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
    }
}