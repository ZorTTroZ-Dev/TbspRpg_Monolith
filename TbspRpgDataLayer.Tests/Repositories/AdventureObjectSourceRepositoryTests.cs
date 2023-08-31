using System;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories;

public class AdventureObjectSourceRepositoryTests: InMemoryTest
{
    public AdventureObjectSourceRepositoryTests() : base("AdventureObjectSourceRepositoryTests")
    {
    }

    #region GetAdventureObjectsWithSourceById

    [Fact]
    public async void GetAdventureObjectsWithSourceById_InvalidIds_ReturnEmpty()
    {
        // arrange
        var nameSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "name"
        };
        var descriptionSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "description"
        };
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            NameSourceKey = nameSource.Key,
            DescriptionSourceKey = descriptionSource.Key
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.SourcesEn.AddRangeAsync(new En[]
        {
            nameSource,
            descriptionSource
        });
        await context.AdventureObjects.AddAsync(testObject);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectSourceRepository(context);

        // act
        var adventureObjectSources = await repository.GetAdventureObjectsWithSourceById(
            new Guid[] {Guid.NewGuid()}, Languages.ENGLISH);

        // assert
        Assert.Empty(adventureObjectSources);
    }
    
    [Fact]
    public async void GetAdventureObjectsWithSourceById_BadLanguage_ThrowException()
    {
        // arrange
        var nameSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "name"
        };
        var descriptionSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "description"
        };
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            NameSourceKey = nameSource.Key,
            DescriptionSourceKey = descriptionSource.Key
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.SourcesEn.AddRangeAsync(new En[]
        {
            nameSource,
            descriptionSource
        });
        await context.AdventureObjects.AddAsync(testObject);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectSourceRepository(context);
        
        //act
        Task Act() => repository.GetAdventureObjectsWithSourceById(
            new Guid[] {testObject.Id}, "banana");
        
        //assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }
    
    [Fact]
    public async void GetAdventureObjectsWithSourceById_ValidIds_ReturnAdventureObjectSource()
    {
        // arrange
        var nameSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "name"
        };
        var descriptionSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "description"
        };
        var nameTwoSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "nametwo"
        };
        var descriptionTwoSource = new En()
        {
            Id = Guid.NewGuid(),
            Key = Guid.NewGuid(),
            Language = Languages.ENGLISH,
            Text = "descriptiontwo"
        };
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            NameSourceKey = nameSource.Key,
            DescriptionSourceKey = descriptionSource.Key
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            NameSourceKey = nameTwoSource.Key,
            DescriptionSourceKey = descriptionTwoSource.Key
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.SourcesEn.AddRangeAsync(new En[]
        {
            nameSource,
            descriptionSource,
            nameTwoSource,
            descriptionTwoSource
        });
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectSourceRepository(context);

        // act
        var adventureObjectSources = await repository.GetAdventureObjectsWithSourceById(
            new Guid[] {testObject.Id}, Languages.ENGLISH);

        // assert
        Assert.NotNull(adventureObjectSources);
        Assert.Single(adventureObjectSources);
        Assert.Equal(testObject.Id, adventureObjectSources.First().AdventureObject.Id);
        Assert.Equal("name", adventureObjectSources.First().NameSource.Text);
        Assert.Equal("description", adventureObjectSources.First().DescriptionSource.Text);
    }

    #endregion
}