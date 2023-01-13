using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors;

public class AdventureObjectProcessorTests: ProcessorTest
{
    #region RemoveAdventure

        [Fact]
        public async void RemoveAdventureObject_BadAdventureObjectId_ExceptionThrown()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                }
            };
            var testAdventureObjects = new List<AdventureObject>()
            {
                new AdventureObject()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id
                }
            };
                
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                testAdventures,
                null,
                null,
                null,
                null,
                null,
                testAdventureObjects);
            
            // act
            Task Act() => processor.RemoveAdventureObject(new AdventureObjectRemoveModel()
            {
                AdventureObjectId = Guid.NewGuid()
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void RemoveAdventureObject_Valid_AdventureObjectRemoved()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                }
            };
            var testAdventureObjects = new List<AdventureObject>()
            {
                new AdventureObject()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id
                }
            };
                
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                testAdventures,
                null,
                null,
                null,
                null,
                null,
                testAdventureObjects);
            
            // act
            await processor.RemoveAdventureObject(new AdventureObjectRemoveModel()
            {
                AdventureObjectId = testAdventureObjects[0].Id
            });
            
            // assert
            Assert.Empty(testAdventureObjects);
        }

        #endregion
}