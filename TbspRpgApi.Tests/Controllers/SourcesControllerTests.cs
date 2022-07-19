using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class SourcesControllerTests: ApiTest
    {
        private SourcesController CreateController(ICollection<En> sources)
        {
            var service = CreateSourcesService(sources, Guid.Empty);
            return new SourcesController(service,
                MockPermissionService(),
                NullLogger<SourcesController>.Instance);
        }

        #region GetSourcesForAdventure

        [Fact]
        public async void GetSourcesForAdventure_NoKey_ReturnList()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.Empty
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourcesForAdventure(
                sources[0].AdventureId, new SourceFilterRequest()
                {
                    Language = Languages.ENGLISH
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModels = okObjectResult.Value as List<SourceViewModel>;
            Assert.NotNull(sourceViewModels);
            Assert.Equal(sources[0].Id, sourceViewModels[0].Id);
        }
        
        [Fact]
        public async void GetSourcesForAdventure_NoLanguageNoKey_ReturnList()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.Empty
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourcesForAdventure(
                sources[0].AdventureId, new SourceFilterRequest());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModels = okObjectResult.Value as List<SourceViewModel>;
            Assert.NotNull(sourceViewModels);
            Assert.Equal(sources[0].Id, sourceViewModels[0].Id);
        }
        
        [Fact]
        public async void GetSourcesForAdventure_EmptyGuidKey_ReturnSource()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.Empty
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourcesForAdventure(
                Guid.NewGuid(), new SourceFilterRequest()
                {
                    Key = Guid.Empty,
                    Language = Languages.ENGLISH
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(sources[0].Id, sourceViewModel.Id);
        }
        
        [Fact]
        public async void GetSourcesForAdventure_Valid_ReturnSource()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourcesForAdventure(
                sources[0].AdventureId, new SourceFilterRequest()
                {
                    Key = sources[0].Key,
                    Language = Languages.ENGLISH
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(sources[0].Id, sourceViewModel.Id);
        }

        #endregion
        
        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_NonEmptyKey_ReturnBadRequest()
        {
            // arrange
            var sources = new List<En>();
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourceForKey(
                new SourceFilterRequest()
                {
                    Key = Guid.NewGuid(),
                    Language = Languages.ENGLISH
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetSourceKey_NoLanguage_ReturnBadRequest()
        {
            // arrange
            var sources = new List<En>();
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourceForKey(
                new SourceFilterRequest()
                {
                    Key = Guid.Empty
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetSourceKey_EmptyGuidKey_ReturnSource()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.Empty
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetSourceForKey(
                new SourceFilterRequest()
                {
                    Key = Guid.Empty,
                    Language = Languages.ENGLISH
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(sources[0].Id, sourceViewModel.Id);
        }
        
        #endregion
        
        #region GetProcessedSourcesForAdventure

        [Fact]
        public async void GetProcessedSourcesForAdventure_NullKey_ReturnBadRequest()
        {
            // arrange
            var sources = new List<En>();
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetProcessedSourcesForAdventure(
                Guid.NewGuid(), new SourceFilterRequest()
                {
                    Language = Languages.ENGLISH
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetProcessedSourcesForAdventure_NoLanguage_ReturnBadRequest()
        {
            // arrange
            var sources = new List<En>();
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetProcessedSourcesForAdventure(
                Guid.NewGuid(), new SourceFilterRequest()
                {
                    Key = Guid.NewGuid()
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetProcessedSourcesForAdventure_EmptyGuidKey_ReturnSource()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.Empty,
                    Text = "banana"
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid(),
                    Text = "banana"
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetProcessedSourcesForAdventure(
                Guid.NewGuid(), new SourceFilterRequest()
                {
                    Key = Guid.Empty,
                    Language = Languages.ENGLISH
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(sources[0].Id, sourceViewModel.Id);
        }
        
        [Fact]
        public async void GetProcessedSourcesForAdventure_Valid_ReturnSource()
        {
            // arrange
            var sources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid(),
                    Text = "banana"
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid(),
                    Text = "banana"
                }
            };
            var controller = CreateController(sources);
            
            // act
            var response = await controller.GetProcessedSourcesForAdventure(
                sources[0].AdventureId, new SourceFilterRequest()
                {
                    Key = sources[0].Key,
                    Language = Languages.ENGLISH
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(sources[0].Id, sourceViewModel.Id);
        }

        #endregion

        #region UpdateSource

        [Fact]
        public async void UpdateSource_Valid_SourceUpdated()
        {
            // arrange
            var testSources = new List<En>()
            {
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                },
                new En()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid()
                }
            };
            var controller = CreateController(testSources);
            
            // act
            var response = await controller.UpdateSource(new SourceUpdateRequest()
            {
                Source = new SourceViewModel()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.Empty,
                    Language = Languages.ENGLISH,
                    Name = "new source",
                    Text = "hello"
                }
            });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        #endregion
    }
}