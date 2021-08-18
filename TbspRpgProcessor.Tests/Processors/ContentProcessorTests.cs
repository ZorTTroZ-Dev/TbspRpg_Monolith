using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class ContentProcessorTests : ProcessorTest
    {
        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_NoGame_ThrowException()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Text = "test source"
            };
            var processor = CreateContentProcessor(
                new List<Game>() {testGame},
                new List<En>() {testSource});

            // act
            Task Act() => processor.GetSourceForKey(Guid.NewGuid(), testSource.Key);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void GetSourceForKey_NullLanguage_SourceReturned()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Text = "test source"
            };
            var processor = CreateContentProcessor(
                new List<Game>() {testGame},
                new List<En>() {testSource});
            
            // act
            var source = await processor.GetSourceForKey(testGame.Id, testSource.Key);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal("test source", source);
        }

        [Fact]
        public async void GetSourceForKey_Valid_SourceReturned()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Text = "test source"
            };
            var processor = CreateContentProcessor(
                new List<Game>() {testGame},
                new List<En>() {testSource});
            
            // act
            var source = await processor.GetSourceForKey(testGame.Id, testSource.Key);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal("test source", source);
        }

        #endregion
    }
}