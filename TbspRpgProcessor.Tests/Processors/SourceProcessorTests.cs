using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class SourceProcessorTests : ProcessorTest
    {
        #region CreateOrUpdateSource

        [Fact]
        public async void CreateOrUpdateSource_BadSourceId_ThrowException()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var processor = CreateSourceProcessor(new List<En>() {testEn});
            
            // act
            Task Act() => processor.CreateOrUpdateSource(new Source()
            {
                Id = testEn.Id,
                AdventureId = testEn.AdventureId,
                Key = Guid.NewGuid()
            }, Languages.ENGLISH);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void CreateOrUpdateSource_EmptyKey_CreateNewSource()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateSourceProcessor(sources);
            
            
            // act
            var dbSource = await processor.CreateOrUpdateSource(new Source()
            {
                AdventureId = testEn.AdventureId,
                Key = Guid.Empty,
                Text = "new source"
            }, Languages.ENGLISH);
            
            // assert
            Assert.NotNull(dbSource);
            Assert.Equal(2, sources.Count);
            Assert.Equal("new source", dbSource.Text);
            Assert.NotEqual(Guid.Empty, dbSource.Key);
        }

        [Fact]
        public async void CreateOrUpdateSource_ExistingSource_UpdateKey()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "original text"
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateSourceProcessor(new List<En>() {testEn});
            
            // act
            var dbSource = await processor.CreateOrUpdateSource(new Source()
            {
                Id = testEn.Id,
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Text = "updated text"
            }, Languages.ENGLISH);
            
            // assert
            Assert.Single(sources);
            Assert.Equal("updated text", dbSource.Text);
        }

        #endregion

        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_DoesntExist_ReturnNull()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "original text"
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateSourceProcessor(sources);
            
            // act
            var source = await processor.GetSourceForKey(new SourceForKeyModel()
            {
                Key = testEn.Key,
                AdventureId = Guid.NewGuid(),
                Language = Languages.ENGLISH
            });
            
            // assert
            Assert.Null(source);
        }

        [Fact]
        public async void GetSourceForKey_Valid_SourceTextIsHtml()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateSourceProcessor(sources);
            
            // act
            var source = await processor.GetSourceForKey(new SourceForKeyModel()
            {
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Language = Languages.ENGLISH
            });
            
            // Assert
            Assert.Equal("<p>This is a text with some <em>emphasis</em></p>", source.Text);
        }

        #endregion
    }
}