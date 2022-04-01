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
            // arrange
            var testAdventureId = Guid.NewGuid();
            var testSources = new List<En>()
            {
                new En()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    Key = Guid.NewGuid(),
                    Text = "source one"
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    Key = Guid.NewGuid(),
                    Text = "source two"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    UserId = testUsers[0].Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    UserId = testUsers[0].Id
                }
            };
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 1,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[1].Id,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[1].Id,
                    Position = 1,
                    SourceKey = Guid.NewGuid()
                }
            };
            var locationId = Guid.NewGuid();
            var locationIdTwo = Guid.NewGuid();
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = testAdventureId,
                    Name = "test adventure",
                    Games = testGames,
                    Locations = new List<Location>()
                    {
                        new Location()
                        {
                            Id = locationId,
                            Name = "test location",
                            Initial = true,
                            SourceKey = Guid.NewGuid(),
                            Routes = new List<Route>()
                            {
                                new Route()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "test route",
                                    LocationId = locationId
                                }
                            }
                        },
                        new Location()
                        {
                            Id = locationIdTwo,
                            Name = "test location two",
                            Initial = true,
                            SourceKey = Guid.NewGuid(),
                            Routes = new List<Route>()
                            {
                                new Route()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "test route",
                                    LocationId = locationIdTwo
                                }
                            }
                        }
                    }
                }
            };
            var processor = CreateAdventureProcessor(
                testAdventures,
                testSources,
                testUsers,
                testGames,
                testAdventures[0].Locations,
                testContents);
            
            // act
            Task Act() => processor.RemoveAdventure(new AdventureRemoveModel()
            {
                AdventureId = Guid.NewGuid()
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void RemoveAdventure_Valid_AdventureRemoved()
        {
            // arrange
            var testAdventureId = Guid.NewGuid();
            var testSources = new List<En>()
            {
                new En()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    Key = Guid.NewGuid(),
                    Text = "source one"
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    Key = Guid.NewGuid(),
                    Text = "source two"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    UserId = testUsers[0].Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventureId,
                    UserId = testUsers[0].Id
                }
            };
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 1,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[1].Id,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[1].Id,
                    Position = 1,
                    SourceKey = Guid.NewGuid()
                }
            };
            var locationId = Guid.NewGuid();
            var locationIdTwo = Guid.NewGuid();
            var testRoutes = new List<Route>
            {
                new Route()
                {
                    Id = Guid.NewGuid(),
                    Name = "test route",
                    LocationId = locationId
                },
                new Route()
                {
                    Id = Guid.NewGuid(),
                    Name = "test route",
                    LocationId = locationIdTwo
                }
            };
            var testLocations = new List<Location>()
            {
                new Location()
                {
                    Id = locationId,
                    Name = "test location",
                    Initial = true,
                    SourceKey = Guid.NewGuid(),
                    Routes = new List<Route>()
                    {
                        testRoutes[0]
                    }
                },
                new Location()
                {
                    Id = locationIdTwo,
                    Name = "test location two",
                    Initial = false,
                    SourceKey = Guid.NewGuid(),
                    Routes = new List<Route>()
                    {
                        testRoutes[1]
                    }
                }
            };
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = testAdventureId,
                    Name = "test adventure",
                    Games = new List<Game>() { testGames[0], testGames[1] },
                    Locations = new List<Location>() { testLocations[0], testLocations[1] }
                }
            };
            var processor = CreateAdventureProcessor(
                testAdventures,
                testSources,
                testUsers,
                testGames,
                testLocations,
                testContents,
                testRoutes);
            
            // act
            await processor.RemoveAdventure(new AdventureRemoveModel()
            {
                AdventureId = testAdventureId
            });
            
            // assert
            Assert.Empty(testAdventures);
            Assert.Empty(testLocations);
            Assert.Empty(testRoutes);
            Assert.Empty(testSources);
            Assert.Empty(testContents);
        }

        #endregion
    }
}