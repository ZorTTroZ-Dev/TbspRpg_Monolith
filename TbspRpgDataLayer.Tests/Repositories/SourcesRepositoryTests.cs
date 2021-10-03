using System;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
{
    public class SourcesRepositoryTests : InMemoryTest
    {
        public SourcesRepositoryTests() : base("SourceRepositoryTests") { }

        #region GetSourceTextForKey

        [Fact]
        public async void GetSourceTextForKey_Valid_SourceReturned()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEn.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            //act
            var text = await repository.GetSourceTextForKey(testSource.Key);
            
            //assert
            Assert.Equal(testSource.Text, text);
        }

        [Fact]
        public async void GetSourceTextForKey_InvalidKey_ReturnNone()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEn.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            //act
            var text = await repository.GetSourceTextForKey(Guid.NewGuid());
            
            //assert
            Assert.Null(text);
        }
        
        [Fact]
        public async void GetSourceTextForKey_ChangeLanguage_SourceReturnedInLanguage()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "test spanish",
                Text = "in spanish"
            };
            context.SourcesEsp.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            //act
            var text = await repository.GetSourceTextForKey(testSource.Key, Languages.SPANISH);
            
            //assert
            Assert.Equal(testSource.Text, text);
        }
        
        [Fact]
        public async void GetSourceTextForKey_InvalidLanguage_ThrowException()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEn.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
        
            //act
            Task Act() => repository.GetSourceTextForKey(testSource.Key, "banana");
        
            //assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion

        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_InvalidAdventureId_ReturnNull()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEsp.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
        
            //act
            var source = await repository.GetSourceForKey(
                testSource.Key, Guid.NewGuid(), Languages.SPANISH);
            
            // assert
            Assert.Null(source);
        }
        
        [Fact]
        public async void GetSourceForKey_NullLanguage_ReturnEnglish()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            var testSourceEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = testSource.AdventureId,
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
        
            //act
            var source = await repository.GetSourceForKey(
                testSource.Key, testSource.AdventureId, null);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(testSourceEn.Id, source.Id);
        }
        
        [Fact]
        public async void GetSourceForKey_InvalidKey_ReturnNull()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEsp.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
        
            //act
            var source = await repository.GetSourceForKey(
                Guid.NewGuid(), testSource.AdventureId, Languages.SPANISH);
            
            // assert
            Assert.Null(source);
        }
        
        [Fact]
        public async void GetSourceForKey_Valid_ReturnSource()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new Esp()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEsp.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
        
            //act
            var source = await repository.GetSourceForKey(
                testSource.Key, testSource.AdventureId, Languages.SPANISH);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(testSource.Id, source.Id);
        }
        
        [Fact]
        public async void GetSourceForKey_InValidLanguage_ThrowException()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test source",
                Text = "test source"
            };
            context.SourcesEn.Add(testSource);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
        
            //act
            Task Act() => repository.GetSourceForKey(
                testSource.Key, testSource.AdventureId,"banana");
        
            //assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion

        #region AddSource

        [Fact]
        public async void AddSource_NoLanguage_AddEnglish()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var source = new Source()
            {
                Id = Guid.NewGuid(),
                Text = "default"
            };
            var repository = new SourcesRepository(context);
            
            // act
            await repository.AddSource(source, null);
            await context.SaveChangesAsync();

            // assert
            Assert.Single(context.SourcesEn);
        }

        [Fact]
        public async void AddSource_English_AddEnglish()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var source = new Source()
            {
                Id = Guid.NewGuid(),
                Text = "default"
            };
            var repository = new SourcesRepository(context);
            
            // act
            await repository.AddSource(source, Languages.ENGLISH);
            await context.SaveChangesAsync();

            // assert
            Assert.Single(context.SourcesEn);
        }

        [Fact]
        public async void AddSource_BadLanguage_ThrowException()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var source = new Source()
            {
                Id = Guid.NewGuid(),
                Text = "default"
            };
            var repository = new SourcesRepository(context);
            
            // act
            Task Act() => repository.AddSource(source, "banana");
        
            //assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion
    }
}