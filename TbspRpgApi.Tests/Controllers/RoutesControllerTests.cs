﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class RoutesControllerTests : ApiTest
    {
        private RoutesController CreateController(ICollection<Route> routes, Guid? exceptionId = null)
        {
            var service = CreateRoutesService(routes, exceptionId);
            return new RoutesController(service, NullLogger<RoutesController>.Instance);
        }

        #region GetRoutesForLocation

        [Fact]
        public async void GetRoutesForLocation_RouteWithLocationExists_ReturnRoutes()
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
            var controller = CreateController(testRoutes);
            
            // act
            var response = await controller.GetRoutesForLocation(testRoutes[0].LocationId);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var routeViewModels = okObjectResult.Value as List<RouteViewModel>;
            Assert.NotNull(routeViewModels);
            Assert.Single(routeViewModels);
            Assert.Equal(testRoutes[0].Id, routeViewModels[0].Id);
        }
        
        [Fact]
        public async void GetRoutesForLocation_RouteWithLocationNotExists_ReturnEmpty()
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
            var controller = CreateController(testRoutes);
            
            // act
            var response = await controller.GetRoutesForLocation(Guid.NewGuid());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var routeViewModels = okObjectResult.Value as List<RouteViewModel>;
            Assert.NotNull(routeViewModels);
            Assert.Empty(routeViewModels);
        }

        #endregion

        #region GetRoutesWithDestination

        [Fact]
        public async void GetRoutesWithDestination_RouteWithDestinationExists_ReturnRoutes()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testRoutes);
            
            // act
            var response = await controller.GetRoutesWithDestination(testRoutes[0].DestinationLocationId);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var routeViewModels = okObjectResult.Value as List<RouteViewModel>;
            Assert.NotNull(routeViewModels);
            Assert.Single(routeViewModels);
            Assert.Equal(testRoutes[0].Id, routeViewModels[0].Id);
        }
        
        [Fact]
        public async void GetRoutesWithDestination_RouteWithDestinationNotExists_ReturnEmpty()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testRoutes);
            
            // act
            var response = await controller.GetRoutesWithDestination(Guid.NewGuid());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var routeViewModels = okObjectResult.Value as List<RouteViewModel>;
            Assert.NotNull(routeViewModels);
            Assert.Empty(routeViewModels);
        }

        #endregion

        #region GetRouteById

        [Fact]
        public async void GetRouteById_RouteExists_ReturnRoute()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testRoutes);
            
            // act
            var response = await controller.GetRouteById(testRoutes[0].Id);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var routeViewModel = okObjectResult.Value as RouteViewModel;
            Assert.NotNull(routeViewModel);
            Assert.Equal(testRoutes[0].Id, routeViewModel.Id);
        }

        [Fact]
        public async void GetRouteById_RouteNotExists_ReturnEmpty()
        {
            // arrange
            var testRoutes = new List<Route>()
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    DestinationLocationId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testRoutes);
            
            // act
            var response = await controller.GetRouteById(Guid.NewGuid());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var routeViewModel = okObjectResult.Value as RouteViewModel;
            Assert.Null(routeViewModel);
        }

        #endregion

        #region UpdateRoutesWithSource

        [Fact]
        public async void UpdateLocationAndSource_Valid_ReturnOk()
        {
            // arrange
            var controller = CreateController(new List<Route>(), Guid.NewGuid());
            
            // act
            var response = await controller.UpdateRoutesWithSource(
                new RouteUpdateRequest[]
                {
                    new()
                    {
                        route = new RouteViewModel(new Route()
                        {
                            Id = Guid.NewGuid(),
                            Name = "route name"
                        }),
                        newDestinationLocationName = "",
                        source = new SourceViewModel(Guid.Empty,"source text"),
                        successSource = new SourceViewModel(Guid.Empty,"success source text")
                    }
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        public async void UpdateLocationAndSource_UpdateFails_ReturnBadRequest()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateController(new List<Route>(), exceptionId);
            
            // act
            var response = await controller.UpdateRoutesWithSource(
                new RouteUpdateRequest[]
                {
                    new()
                    {
                        route = new RouteViewModel(new Route()
                        {
                            Id = exceptionId,
                            Name = "route name"
                        }),
                        newDestinationLocationName = "",
                        source = new SourceViewModel(Guid.Empty, "source text"),
                        successSource = new SourceViewModel(Guid.Empty, "success source text")
                    }
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion
    }
}