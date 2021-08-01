using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class AdventuresServiceTests : ApiTest
    {
        #region GetAllAdventures

        [Fact]
        public async void GetAllAdventures_ReturnsAllAdventures()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test",
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test two",
                    SourceKey = Guid.NewGuid()
                }
            };
            var service = CreateAdventuresService(testAdventures);
            
            // act
            var adventures = await service.GetAllAdventures();
            
            // assert
            Assert.Equal(2, adventures.Count);
            Assert.Equal(testAdventures[0].Id, adventures[0].Id);
        }

        #endregion
    }
}