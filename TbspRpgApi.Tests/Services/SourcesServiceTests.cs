using System;
using System.Collections.Generic;
using TbspRpgApi.Entities.LanguageSources;
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
    }
}