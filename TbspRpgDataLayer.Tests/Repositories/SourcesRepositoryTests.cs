using System;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
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
            var repository = new SourcesRepository(context);
        
            //act
            var source = await repository.GetSourceForKey(
                testSource.Key, Guid.NewGuid(), Languages.SPANISH);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(testSource.Id, source.Id);
        }
        
        [Fact]
        public async void GetSourceForKey_EmptyGuidKeyEmptyAdventure_ReturnSource()
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
            var repository = new SourcesRepository(context);
        
            //act
            var source = await repository.GetSourceForKey(
                testSource.Key, Guid.Empty, Languages.SPANISH);
            
            // assert
            Assert.NotNull(source);
            Assert.Equal(testSource.Id, source.Id);
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

        #region RemoveSource
        
        [Fact]
        public async void RemoveSource_InvalidSourceId_SourceNotRemoved()
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
            var testSourceEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            context.SourcesEn.Add(testSourceEnTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            await repository.RemoveSource(Guid.NewGuid());
            await context.SaveChangesAsync();
            
            // assert
            Assert.Equal(2, context.SourcesEn.Count());
            Assert.Single(context.SourcesEsp);
        }

        [Fact]
        public async void RemoveSource_ValidSourceId_SourceRemoved()
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
            var testSourceEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            context.SourcesEn.Add(testSourceEnTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            await repository.RemoveSource(testSource.Id);
            await context.SaveChangesAsync();
            
            // assert
            Assert.Equal(2, context.SourcesEn.Count());
            Assert.Empty(context.SourcesEsp);
        }

        #endregion
        
        #region RemoveAllSourceForAdventure

        [Fact]
        public async void RemoveAllSourceForAdventure_AllSourceRemoved()
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
            var testSourceEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            context.SourcesEn.Add(testSourceEnTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            await repository.RemoveAllSourceForAdventure(testSource.AdventureId);
            await context.SaveChangesAsync();
            
            // assert
            Assert.Empty(context.SourcesEsp);
            Assert.Single(context.SourcesEn);
        }

        #endregion

        #region GetAllSourceForAdventure

        [Fact]
        public async void GetAllSourceForAdventure_Valid_AllSourceReturned()
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
            var testSourceEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            context.SourcesEn.Add(testSourceEnTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            var source = await repository.GetAllSourceForAdventure(testSource.AdventureId, Languages.ENGLISH);
            
            // assert
            Assert.Single(source);
        }

        #endregion

        #region GetAllSourceAllLanguagesForAdventure

        [Fact]
        public async void GetAllSourceAllLanguagesForAdventure()
        {
            // arrange
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
            var testSourceEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            context.SourcesEn.Add(testSourceEnTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            var sources = await repository.GetAllSourceAllLanguagesForAdventure(testSource.AdventureId);
            
            // assert
            Assert.Equal(2, sources.Count);
            Assert.Equal(Languages.ENGLISH, sources.First(source => source.Id == testSourceEn.Id).Language);
            Assert.Equal(Languages.SPANISH, sources.First(source => source.Id == testSource.Id).Language);
        }

        #endregion

        #region GetSourcesWithScript

        [Fact]
        public async void GetSourcesWithScript_HasSources_ReturnSources()
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
            await context.SourcesEsp.AddRangeAsync(testSource, testSourceTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            var sources = await repository.GetSourcesWithScript(testSource.Script.Id);
            
            // assert
            Assert.Single(sources);
        }

        [Fact]
        public async void GetSourcesWithScript_MultipleLanguages_ReturnSources()
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
            var repository = new SourcesRepository(context);
            
            // act
            var sources = await repository.GetSourcesWithScript(testSource.Script.Id);
            
            // assert
            Assert.Equal(2, sources.Count);
            Assert.NotNull(sources.FirstOrDefault(source => source.Id == testSourceThree.Id));
            Assert.NotNull(sources.FirstOrDefault(source => source.Id == testSource.Id));
        }

        [Fact]
        public async void GetSourcesWithScript_NoSources_ReturnEmpty()
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
            var repository = new SourcesRepository(context);
            
            // act
            var sources = await repository.GetSourcesWithScript(Guid.NewGuid());
            
            // assert
            Assert.Empty(sources);
        }

        #endregion
        
        #region GetSourceById
        
        [Fact]
        public async void GetSourceById_InvalidSourceId_ReturnNull()
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
            var testSourceEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            context.SourcesEn.Add(testSourceEnTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            var source = await repository.GetSourceById(Guid.NewGuid());
            
            // assert
            Assert.Null(source);
        }

        [Fact]
        public async void GetSourceId_ValidSourceId_SourceReturned()
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
            var testSourceEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = testSource.Key,
                AdventureId = Guid.NewGuid(),
                Name = "test source two",
                Text = "test source two"
            };
            context.SourcesEsp.Add(testSource);
            context.SourcesEn.Add(testSourceEn);
            context.SourcesEn.Add(testSourceEnTwo);
            await context.SaveChangesAsync();
            var repository = new SourcesRepository(context);
            
            // act
            var source = await repository.GetSourceById(testSource.Id);

            // assert
            Assert.NotNull(source);
            Assert.Equal(testSource.Id, source.Id);
        }

        #endregion
    }
}