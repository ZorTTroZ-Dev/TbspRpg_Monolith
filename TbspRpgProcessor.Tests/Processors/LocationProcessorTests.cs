using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class LocationProcessorTests : ProcessorTest
    {
        #region UpdateLocation

        [Fact]
        public async void UpdateLocation_BadLocationId_ThrowException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            Task Act() => processor.UpdateLocation(new Location()
            {
                Id = Guid.NewGuid(),
                Name = "updated location name",
                Initial = false,
                SourceKey = testLocation.SourceKey
            }, new En()
            {
                Id = testSource.Id,
                Key = testSource.Key,
                Name = "test location",
                Text = "updated source"
            }, Languages.ENGLISH);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void UpdateLocation_NewSource_NewSourceCreated()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.Empty
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>();
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            await processor.UpdateLocation(new Location()
            {
                Id = testLocation.Id,
                Name = "updated location name",
                Initial = true
            }, new En()
            {
                Key = Guid.Empty,
                Text = "updated source"
            }, Languages.ENGLISH);

            // assert
            Assert.Single(sources);
            Assert.Single(locations);
            Assert.Equal("updated location name", sources[0].Name);
            Assert.Equal("updated source", sources[0].Text);
            Assert.Equal(sources[0].Key, locations[0].SourceKey);
        }

        [Fact]
        public async void UpdateLocation_BadSourceId_ThrowException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            Task Act() => processor.UpdateLocation(new Location()
            {
                Id = testLocation.Id,
                Name = "updated location name",
                Initial = false,
                SourceKey = testLocation.SourceKey
            }, new En()
            {
                Id = testSource.Id,
                Key = Guid.NewGuid(),
                Name = "test location",
                Text = "updated source"
            }, Languages.ENGLISH);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void UpdateLocation_UpdateLocationAndSource_LocationSourceUpdated()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            await processor.UpdateLocation(new Location()
            {
                Id = testLocation.Id,
                Name = "updated location name",
                Initial = false,
                SourceKey = testLocation.SourceKey
            }, new En()
            {
                Id = testSource.Id,
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "updated source"
            }, Languages.ENGLISH);
            
            // assert
            Assert.Single(sources);
            Assert.Single(locations);
            Assert.False(locations[0].Initial);
            Assert.Equal("updated location name", locations[0].Name);
            Assert.Equal("updated source", sources[0].Text);
        }

        #endregion
    }
}