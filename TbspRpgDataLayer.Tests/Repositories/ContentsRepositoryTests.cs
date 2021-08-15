using System;
using System.Collections.Generic;
using System.Linq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
{
    public class ContentsRepositoryTests : InMemoryTest
    {
        public ContentsRepositoryTests() : base("ContentsRepositoryTests")
        {
        }

        private static IContentsRepository CreateRepository(DatabaseContext context)
        {
            return new ContentsRepository(context);
        }

        #region AddContent

        [Fact]
        public async void AddContent_ContentAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                Position = 42
            };
            var repository = CreateRepository(context);
            
            // act
            await repository.AddContent(testContent);
            await repository.SaveChanges();
            
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
                Position = 42,
                GameId = Guid.NewGuid()
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var repository = CreateRepository(context);
            
            // act
            var content = await repository.GetContentForGameAtPosition(testContent.GameId, 42);
            
            // assert
            Assert.NotNull(content);
            Assert.Equal(testContent.Id, content.Id);
        }
        
        [Fact]
        public async void GetContentForGameAtPosition_InvalidGame_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                Position = 42,
                GameId = Guid.NewGuid()
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var repository = CreateRepository(context);
            
            // act
            var content = await repository.GetContentForGameAtPosition(Guid.NewGuid(), 42);
            
            // assert
            Assert.Null(content);
        }
        
        [Fact]
        public async void GetContentForGameAtPosition_InvalidPosition_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                Position = 42,
                GameId = Guid.NewGuid()
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var repository = CreateRepository(context);
            
            // act
            var content = await repository.GetContentForGameAtPosition(testContent.GameId, 40);
            
            // assert
            Assert.Null(content);
        }

        #endregion

        #region GetContentForGame

        [Fact]
        public async void GetContentForGame_NoCountNoOffset_ReturnsAll()
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(testGameId);
            
            //assert
            Assert.Equal(3, contents.Count);
            Assert.Equal((ulong)0, contents[0].Position);
            Assert.Equal((ulong)42, contents[2].Position);
        }
        
        [Fact]
        public async void GetContentForGame_NoOffset_ReturnPartial()
        {
            // arrange
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(testGameId, null, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)0, contents[0].Position);
            Assert.Equal((ulong)1, contents[1].Position);
        }
        
        [Fact]
        public async void GetContentForGame_NoCount_ReturnPartial()
        {
            // arrange
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(testGameId, 2);
            
            //assert
            Assert.Single(contents);
            Assert.Equal(testContents[0].Id, contents[0].Id);
            Assert.Equal((ulong)42, contents[0].Position);
        }
        
        [Fact]
        public async void GetContentForGame_OffsetCount_ReturnPartial()
        {
            // arrange
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(testGameId, 1, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)1, contents[0].Position);
            Assert.Equal((ulong)42, contents[1].Position);
        }
        
        #endregion
        
        #region GetContentForGameReverse

        [Fact]
        public async void GetContentForGameReverse_NoCountNoOffset_ReturnsAll()
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(testGameId);
            
            //assert
            Assert.Equal(3, contents.Count);
            Assert.Equal((ulong)42, contents[0].Position);
            Assert.Equal((ulong)0, contents[2].Position);
        }
        
        [Fact]
        public async void GetContentForGameReverse_NoOffset_ReturnPartial()
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(testGameId, null, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)42, contents[0].Position);
            Assert.Equal((ulong)1, contents[1].Position);
        }

        [Fact]
        public async void GetContentForGameReverse_NoCount_ReturnPartial()
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(testGameId, 2);
            
            //assert
            Assert.Single(contents);
            Assert.Equal((ulong)0, contents[0].Position);
        }
        
        [Fact]
        public async void GetContentForGameReverse_OffsetCount_ReturnPartial()
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(testGameId, 1, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)1, contents[0].Position);
            Assert.Equal((ulong)0, contents[1].Position);
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGameAfterPosition(testGameId, 40);
            
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
            var repository = new ContentsRepository(context);
            
            //act
            var contents = await repository.GetContentForGameAfterPosition(testGameId, 42);
            
            //assert
            Assert.Empty(contents);
        }

        #endregion
    }
}