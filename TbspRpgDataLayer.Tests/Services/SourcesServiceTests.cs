using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
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

        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_NullLanguage_ReturnDefault()
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
            var source = await service.GetSourceForKey(testEn.Key);
            
            // assert
            Assert.Equal(testEn.Text, source);
        }

        [Fact]
        public async void GetSourceForKey_InvalidKey_ReturnNull()
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
            var source = await service.GetSourceForKey(Guid.NewGuid());
            
            // assert
            Assert.Null(source);
        }

        [Fact]
        public async void GetSourceForKey_InvalidLanguage_ThrowException()
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
            Task Act() => service.GetSourceForKey(testEn.Key, "banana");

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
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
            var source = await service.GetSourceForKey(testEn.Key, Languages.SPANISH);
            
            // assert
            Assert.Equal(testEsp.Text, source);
        }

        #endregion
    }
}