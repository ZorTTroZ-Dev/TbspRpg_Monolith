using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class AdventureProcessorTests: ProcessorTest
    {
        #region UpdateAdventure

        [Fact]
        public async void UpdateAdventure_InvalidAdventureId_ExceptionThrown()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test_adventure",
                InitialSourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testAdventure.InitialSourceKey,
                Name = "test_adventure",
                Text = "test source"
            };
            var adventures = new List<Adventure>() { testAdventure };
            var sources = new List<En>() {testSource};
            var processor = CreateAdventureProcessor(adventures, sources);
            
            // act
            Task Act() => processor.UpdateAdventure(new AdventureUpdateModel()
            {
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid(),
                    Name = "updated_test_adventure"
                },
                InitialSource = new En()
                {
                    Id = testSource.Id,
                    Key = testSource.Key,
                    Name = testSource.Name,
                    Text = testSource.Text
                },
                UserId = Guid.NewGuid(),
                Language = Languages.ENGLISH
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void UpdateAdventure_EmptyAdventureId_AdventureCreated()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test_adventure",
                InitialSourceKey = Guid.NewGuid(),
                DescriptionSourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testAdventure.InitialSourceKey,
                Name = "test_adventure",
                Text = "test source",
                AdventureId = testAdventure.Id
            };
            var testDescriptionSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testAdventure.DescriptionSourceKey,
                Name = "description_test_adventure",
                Text = "test description source",
                AdventureId = testAdventure.Id
            };
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "tester"
            };
            var adventures = new List<Adventure>() { testAdventure };
            var sources = new List<En>() {testSource, testDescriptionSource};
            var processor = CreateAdventureProcessor(adventures, sources);
            
            // act
            await processor.UpdateAdventure(new AdventureUpdateModel()
            {
                Adventure = new Adventure()
                {
                    Id = Guid.Empty,
                    Name = "new_test_adventure",
                    InitialSourceKey = Guid.Empty,
                    PublishDate = DateTime.UtcNow
                },
                InitialSource = new En()
                {
                    Key = Guid.Empty,
                    Text = "new_test source"
                },
                DescriptionSource = new En()
                {
                    Key = Guid.Empty,
                    Text = "new_test description source"
                },
                UserId = testUser.Id,
                Language = Languages.ENGLISH
            });

            // assert
            Assert.Equal(2, adventures.Count);
            var newAdventure = adventures.FirstOrDefault(adv => adv.Name == "new_test_adventure");
            Assert.NotNull(newAdventure);
            Assert.Equal(testUser.Id, newAdventure.CreatedByUserId);
            Assert.Equal(4, sources.Count);
        }

        [Fact]
        public async void UpdateAdventure_ExistingAdventure_AdventureUpdated()
        {
            // arrange
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "tester"
            };
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test_adventure",
                InitialSourceKey = Guid.NewGuid(),
                DescriptionSourceKey = Guid.NewGuid(),
                CreatedByUserId = testUser.Id
            };
            var testDescriptionSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testAdventure.DescriptionSourceKey,
                Name = "description_test_adventure",
                Text = "test description source",
                AdventureId = testAdventure.Id
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testAdventure.InitialSourceKey,
                Name = "test_adventure",
                Text = "test source",
                AdventureId = testAdventure.Id
            };
            var adventures = new List<Adventure>() { testAdventure };
            var sources = new List<En>() {testSource, testDescriptionSource};
            var processor = CreateAdventureProcessor(adventures, sources);
            
            // act
            await processor.UpdateAdventure(new AdventureUpdateModel()
            {
                Adventure = new Adventure()
                {
                    Id = testAdventure.Id,
                    Name = "updated_adventure_name",
                    InitialSourceKey = testAdventure.InitialSourceKey,
                    DescriptionSourceKey = testAdventure.DescriptionSourceKey,
                    CreatedByUserId = testAdventure.CreatedByUserId,
                    PublishDate = DateTime.UtcNow
                },
                InitialSource = new En()
                {
                    Id = testSource.Id,
                    Key = testSource.Key,
                    Name = testSource.Name,
                    Text = "updated source"
                },
                DescriptionSource = new En()
                {
                    Id = testDescriptionSource.Id,
                    Key = testDescriptionSource.Key,
                    Name = testDescriptionSource.Name,
                    Text = "updated description source"
                },
                UserId = testUser.Id,
                Language = Languages.ENGLISH
            });

            // assert
            Assert.Single(adventures);
            Assert.Equal("updated_adventure_name", adventures[0].Name);
            Assert.Equal(2, sources.Count);
            Assert.Equal("updated source", sources.
                First(source => source.Id == testSource.Id).Text);
            Assert.Equal("updated description source", sources.
                First(source => source.Id == testDescriptionSource.Id).Text);
        }

        #endregion

        #region RemoveAdventure

        [Fact]
        public async void RemoveAdventure_BadAdventureId_ExceptionThrown()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async void RemoveAdventure_Valid_AdventureRemoved()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}