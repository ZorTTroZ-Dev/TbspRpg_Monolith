using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class SourcesServiceTests : InMemoryTest
    {
        public SourcesServiceTests() : base("SourcesServiceTests") {}

        private static ISourcesService CreateService(DatabaseContext context)
        {
            return new SourcesService(
                new SourcesRepository(context),
                NullLogger<SourcesService>.Instance);
        }

        #region GetSourceTextForKey

        [Fact]
        public async void GetSourceTextForKey_NullLanguage_ReturnDefault()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "english",
                Text = "english"
            };
            var testEsp = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = testEn.Key,
                Name = "spanish",
                Text = "spanish"
            };
            context.SourcesEn.Add(testEn);
            context.SourcesEsp.Add(testEsp);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var source = await service.GetSourceTextForKey(testEn.Key);
            
            // assert
            Assert.Equal(testEn.Text, source);
        }

        [Fact]
        public async void GetSourceTextForKey_InvalidKey_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "english",
                Text = "english"
            };
            var testEsp = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = testEn.Key,
                Name = "spanish",
                Text = "spanish"
            };
            context.SourcesEn.Add(testEn);
            context.SourcesEsp.Add(testEsp);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var source = await service.GetSourceTextForKey(Guid.NewGuid());
            
            // assert
            Assert.Null(source);
        }

        [Fact]
        public async void GetSourceTextForKey_InvalidLanguage_ThrowException()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "english",
                Text = "english"
            };
            var testEsp = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = testEn.Key,
                Name = "spanish",
                Text = "spanish"
            };
            context.SourcesEn.Add(testEn);
            context.SourcesEsp.Add(testEsp);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            Task Act() => service.GetSourceTextForKey(testEn.Key, "banana");

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void GetSourceTextForKey_Valid_ReturnSource()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "english",
                Text = "english"
            };
            var testEsp = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = testEn.Key,
                Name = "spanish",
                Text = "spanish"
            };
            context.SourcesEn.Add(testEn);
            context.SourcesEsp.Add(testEsp);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var source = await service.GetSourceTextForKey(testEn.Key, Languages.SPANISH);
            
            // assert
            Assert.Equal(testEsp.Text, source);
        }

        #endregion

        #region GetSourceForKey
        
        [Fact]
        public async void GetSourceForKey_EmptyGuidKey_ReturnSource()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.Empty,
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEsp.Add(testSource);
            await context.SaveChangesAsync();
            var service = CreateService(context);
        
            //act
            var source = await service.GetSourceForKey(
                testSource.Key, Guid.NewGuid(), Languages.SPANISH);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(testSource.Id, source.Id);
        }

        [Fact]
        public async void GetSourceForKey_Valid_ReturnSource()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "english",
                Text = "english"
            };
            var testEsp = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Name = "spanish",
                Text = "spanish"
            };
            context.SourcesEn.Add(testEn);
            context.SourcesEsp.Add(testEsp);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var source = await service.GetSourceForKey(testEn.Key, testEn.AdventureId, Languages.SPANISH);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(testEsp.Id, source.Id);
            Assert.Equal(testEsp.Text, source.Text);
        }

        #endregion

        #region AddSource

        [Fact]
        public async void AddSource_SourceAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var source = new Source()
            {
                Id = Guid.NewGuid(),
                Text = "source text"
            };
            var service = CreateService(context);
            
            // act
            await service.AddSource(source, Languages.ENGLISH);
            await context.SaveChangesAsync();

            // assert
            Assert.Single(context.SourcesEn);
        }

        #endregion

        #region RemoveScriptFromSources

        [Fact]
        public async void RemoveScriptFromSources_ScriptRemoved()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source",
                Script = new Script()
                {
                    Id = Guid.NewGuid(),
                    Name = "test script"
                }
            };
            var testSourceTwo = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            var testSourceThree = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source",
                Script = testSource.Script
            };
            await context.SourcesEsp.AddRangeAsync(testSource, testSourceTwo);
            await context.SourcesEn.AddRangeAsync(testSourceThree);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            service.RemoveScriptFromSources(testSource.Script.Id);
            await context.SaveChangesAsync();
            
            // assert
            Assert.Null(context.SourcesEn.First(s => s.Id == testSourceThree.Id).Script);
            Assert.Null(context.SourcesEsp.First(s => s.Id == testSource.Id).Script);
        }

        #endregion
    }
}