using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class ContentsServiceTests : InMemoryTest
    {
        public ContentsServiceTests() : base("ContentsServiceTests")
        {
        }

        private static IContentsService CreateService(DatabaseContext context)
        {
            return new ContentsService(new ContentsRepository(context),
                NullLogger<ContentsService>.Instance);
        }

        #region AddContent

        [Fact]
        public async void AddContent_NotExist_ContentAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            var service = CreateService(context);
            
            // act
            await service.AddContent(testContent);
            await service.SaveChanges();
            
            // assert
            Assert.Single(context.Contents);
            Assert.Equal(testContent.Id, context.Contents.First().Id);
        }

        [Fact]
        public async void AddContent_Exists_ContentNotAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            await service.AddContent(testContent);
            await service.SaveChanges();
            
            // assert
            Assert.Single(context.Contents);
            Assert.Equal(testContent.Id, context.Contents.First().Id);
        }

        #endregion

        #region GetContentForGameAtPosition

        [Fact]
        public async void GetContentForGameAtPosition_Exists_ReturnContent()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var content = await service.GetContentForGameAtPosition(testContent.GameId, 42);
            
            // assert
            Assert.NotNull(content);
            Assert.Equal(testContent.Id, content.Id);
        }

        [Fact]
        public async void GetContentForGameAtPosition_NotExist_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var content = await service.GetContentForGameAtPosition(Guid.NewGuid(), 42);
            
            // assert
            Assert.Null(content);
        }

        #endregion
        
        #region GetAllContent

        [Fact]
        public async void GetAllContentForGame_GetsAllContent()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetAllContentForGame(testGameId);
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(testContents[1].Id, gameContents.First().Id);
        }

        #endregion
        
        #region GetLatestForGame

        [Fact]
        public async void GetLatestForGame_GetsLatest()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetLatestForGame(testGameId);
            
            //assert
            Assert.Equal(testGameId, gameContents.GameId);
            Assert.Equal(testContents[0].Id, gameContents.Id);
        }

        #endregion
        
        #region GetPartialContentForGame

        [Fact]
        public async void GetPartialContentForGame_NoDirection_ContentsForward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest());
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(testContents[1].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_Forward_ContentsForward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "f"
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(testContents[1].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardStart_PartialContentsForward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Start = 2
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Single(gameContents);
            Assert.Equal(testContents[0].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardCountStart_PartialContentsForward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Start = 1,
                Count = 2
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(testContents[2].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardCount_PartialContentsForward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Count = 2
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(testContents[1].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_Backward_ContentsBackward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "b"
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(testContents[0].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardStart_PartialContentsBackward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "b",
                Start = 1
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(testContents[2].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardCount_PartialContentsBackward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "b",
                Count = 2
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(testContents[0].Id, gameContents.First().Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardStartCount_PartialContentsBackward()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
            {
                Direction = "b",
                Start = 1,
                Count = 2
            });
            
            //assert
            Assert.Equal(testGameId, gameContents.First().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(testContents[1].Id, gameContents[1].Id);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BadDirection_Error()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);

            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetPartialContentForGame(testGameId, new ContentFilterRequest()
                {
                    Direction = "zebra",
                    Start = -3,
                    Count = 2
                }));
        }
        
        #endregion
        
        #region GetContentForGameAfterPosition

        [Fact]
        public async void GetContentForGameAfterPosition_EarlyPosition_ReturnContent()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var contents = await service.GetContentForGameAfterPosition(testGameId, 40);
            
            //assert
            Assert.Single(contents);
            Assert.Equal(testContents[0].Id, contents[0].Id);
        }
        
        [Fact]
        public async void GetContentForGameAfterPosition_LastPosition_ReturnNoContent()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 1
                }
            };
            context.Contents.AddRange(testContents);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            //act
            var contents = await service.GetContentForGameAfterPosition(testGameId, 42);
            
            //assert
            Assert.Empty(contents);
        }
        
        #endregion
    }
}