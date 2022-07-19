using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class SourcesServiceTests: ApiTest
    {
        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_DoestExist_ReturnNull()
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
            var service = CreateSourcesService(sources, Guid.Empty);
            
            // act
            var source = await service.GetSourceForKey(
                Guid.NewGuid(), Guid.NewGuid(), Languages.ENGLISH);
            
            // assert
            Assert.Null(source);
        }
        
        [Fact]
        public async void GetSourceForKey_Exists_ReturnSource()
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
            var service = CreateSourcesService(sources, Guid.Empty);
            
            // act
            var source = await service.GetSourceForKey(
                sources[0].Key, sources[0].AdventureId, Languages.ENGLISH);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(sources[0].Id, source.Id);
        }

        #endregion
        
        #region GetProcessedSourceForKey

        [Fact]
        public async void GetProcessedSourceForKey_DoestExist_ReturnNull()
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
            var service = CreateSourcesService(sources, Guid.Empty);
            
            // act
            var source = await service.GetProcessedSourceForKey(
                Guid.NewGuid(), Guid.NewGuid(), Languages.ENGLISH);
            
            // assert
            Assert.Null(source);
        }
        
        [Fact]
        public async void GetProcessedSourceForKey_Exists_ReturnSource()
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
            var service = CreateSourcesService(sources, Guid.Empty);
            
            // act
            var source = await service.GetProcessedSourceForKey(
                sources[0].Key, sources[0].AdventureId, Languages.ENGLISH);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(sources[0].Id, source.Id);
        }

        #endregion

        #region GetSourcesForAdventure

        [Fact]
        public async void GetSourcesForAdventure_Valid_ReturnList()
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
            var service = CreateSourcesService(testSources, Guid.Empty);
            
            // act
            var sources = await service.GetSourcesForAdventure(testSources[0].AdventureId, Languages.ENGLISH);
            
            // assert
            Assert.Single(sources);
        }

        [Fact]
        public async void GetSourcesForAdventure_Invalid_ReturnEmpty()
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
            var service = CreateSourcesService(testSources, Guid.Empty);
            
            // act
            var sources = await service.GetSourcesForAdventure(Guid.NewGuid(), Languages.ENGLISH);
            
            // assert
            Assert.Empty(sources);
        }
        
        #endregion
        
        #region GetSourceAllLanguagesForAdventure

        [Fact]
        public async void GetSourceAllLanguagesForAdventure_Invalid_ReturnEmpty()
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
            var service = CreateSourcesService(testSources, Guid.Empty);
            
            // act
            var sources = await service.GetSourceAllLanguagesForAdventure(Guid.NewGuid());
            
            // assert
            Assert.Empty(sources);
        }

        [Fact]
        public async void GetSourceAllLanguagesForAdventure_Valid_ReturnList()
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
            var service = CreateSourcesService(testSources, Guid.Empty);
            
            // act
            var sources = await service.GetSourceAllLanguagesForAdventure(testSources[0].AdventureId);
            
            // assert
            Assert.Single(sources);
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
            var service = CreateSourcesService(testSources, Guid.Empty);
            
            // act
            await service.UpdateSource(new SourceUpdateRequest()
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
        }

        #endregion
    }
}