using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
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
                Source = new En()
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
                InitialSourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testAdventure.InitialSourceKey,
                Name = "test_adventure",
                Text = "test source",
                AdventureId = testAdventure.Id
            };
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "tester"
            };
            var adventures = new List<Adventure>() { testAdventure };
            var sources = new List<En>() {testSource};
            var processor = CreateAdventureProcessor(adventures, sources);
            
            // act
            await processor.UpdateAdventure(new AdventureUpdateModel()
            {
                Adventure = new Adventure()
                {
                    Id = Guid.Empty,
                    Name = "new_test_adventure",
                    InitialSourceKey = Guid.Empty
                },
                Source = new En()
                {
                    Key = Guid.Empty,
                    Text = "new_test source"
                },
                UserId = testUser.Id,
                Language = Languages.ENGLISH
            });

            // assert
            Assert.Equal(2, adventures.Count);
            var newAdventure = adventures.FirstOrDefault(adv => adv.Name == "new_test_adventure");
            Assert.NotNull(newAdventure);
            Assert.Equal(testUser.Id, newAdventure.CreatedByUserId);
            Assert.Equal(2, sources.Count);
        }

        [Fact]
        public async void UpdateAdventure_ExistingAdventure_AdventureUpdated()
        {
            // arrange
            var testUser = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "tester"
            };
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test_adventure",
                InitialSourceKey = Guid.NewGuid(),
                CreatedByUserId = testUser.Id
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
            var sources = new List<En>() {testSource};
            var processor = CreateAdventureProcessor(adventures, sources);
            
            // act
            await processor.UpdateAdventure(new AdventureUpdateModel()
            {
                Adventure = new Adventure()
                {
                    Id = testAdventure.Id,
                    Name = "updated_adventure_name",
                    InitialSourceKey = testAdventure.InitialSourceKey,
                    CreatedByUserId = testAdventure.CreatedByUserId
                },
                Source = new En()
                {
                    Id = testSource.Id,
                    Key = testSource.Key,
                    Name = testSource.Name,
                    Text = "updated source"
                },
                UserId = testUser.Id,
                Language = Languages.ENGLISH
            });

            // assert
            Assert.Single(adventures);
            Assert.Equal("updated_adventure_name", adventures[0].Name);
            Assert.Single(sources);
            Assert.Equal("updated source", sources[0].Text);
        }

        #endregion
    }
}