using System;
using System.Threading.Tasks;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
{
    public class SourcesRepositoryTests : InMemoryTest
    {
        public SourcesRepositoryTests() : base("SourceRepositoryTests") { }

        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_Valid_SourceReturned()
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
            var text = await repository.GetSourceForKey(testSource.Key);
            
            //assert
            Assert.Equal(testSource.Text, text);
        }

        [Fact]
        public async void GetSourceForKey_InvalidKey_ReturnNone()
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
            var text = await repository.GetSourceForKey(Guid.NewGuid());
            
            //assert
            Assert.Null(text);
        }
        
        [Fact]
        public async void GetSourceForKey_ChangeLanguage_SourceReturnedInLanguage()
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
            var text = await repository.GetSourceForKey(testSource.Key, Languages.SPANISH);
            
            //assert
            Assert.Equal(testSource.Text, text);
        }
        
        [Fact]
        public async void GetSourceForKey_InvalidLanguage_ThrowException()
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
            Task Act() => repository.GetSourceForKey(testSource.Key, "banana");
        
            //assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion
    }
}