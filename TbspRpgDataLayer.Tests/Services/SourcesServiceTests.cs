using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
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

        #region CreateOrUpdateSource

        [Fact]
        public async void CreateOrUpdateSource_BadSourceId_ThrowException()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            await context.SourcesEn.AddAsync(testEn);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            Task Act() => service.CreateOrUpdateSource(new Source()
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
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            await context.SourcesEn.AddAsync(testEn);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var dbSource = await service.CreateOrUpdateSource(new Source()
            {
                AdventureId = testEn.AdventureId,
                Key = Guid.Empty,
                Text = "new source"
            }, Languages.ENGLISH);
            
            // assert
            await context.SaveChangesAsync();
            Assert.NotNull(dbSource);
            Assert.Equal(2, context.SourcesEn.Count());
            Assert.Equal("new source", dbSource.Text);
            Assert.NotEqual(Guid.Empty, dbSource.Key);
        }

        [Fact]
        public async void CreateOrUpdateSource_ExistingSource_UpdateKey()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "original text"
            };
            await context.SourcesEn.AddAsync(testEn);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var dbSource = await service.CreateOrUpdateSource(new Source()
            {
                Id = testEn.Id,
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Text = "updated text"
            }, Languages.ENGLISH);
            
            // assert
            await context.SaveChangesAsync();
            Assert.Single(context.SourcesEn);
            Assert.Equal("updated text", dbSource.Text);
        }

        #endregion
    }
}