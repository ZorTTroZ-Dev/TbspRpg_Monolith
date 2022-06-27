using System.Text.Json;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgDataLayer.Tests.Entities;

public class GameTests
{
    #region GetGameStateProperty

    [Fact]
    public void GetGameStateProperty_ValidKey_PropertyReturned()
    {
        // arrange
        var testGame = new Game()
        {
            GameState = "{\"number\": 42, \"string\": \"banana\", \"boolean\": true}"
        };
        
        // act
        var numberValue = testGame.GetGameStatePropertyNumber("number");
        var stringValue = testGame.GetGameStatePropertyString("string");
        var boolValue = testGame.GetGameStatePropertyBoolean("boolean");
        
        // assert
        Assert.Equal(42, numberValue);
        Assert.Equal("banana", stringValue);
        Assert.True(boolValue);
    }

    [Fact]
    public void GetGameStateProperty_InvalidJson_ExceptionThrown()
    {
        // arrange
        var testGame = new Game()
        {
            GameState = "{\"number\": 42, \"string\": \"banana\", \"boolean\": true"
        };
        
        // act & assert
        Assert.Throws<JsonException>(() => testGame.GetGameStatePropertyNumber("number"));
    }

    [Fact]
    public void GetGameStateProperty_InvalidKey_ReturnDefault()
    {
        // arrange
        var testGame = new Game()
        {
            GameState = "{\"number\": 42, \"string\": \"banana\", \"boolean\": true}"
        };
        
        // act
        var numberValue = testGame.GetGameStatePropertyNumber("numberx");
        var stringValue = testGame.GetGameStatePropertyString("stringx");
        var boolValue = testGame.GetGameStatePropertyBoolean("booleanx");
        
        // assert
        Assert.Equal(0, numberValue);
        Assert.Null(stringValue);
        Assert.False(boolValue);
    }
    
    [Fact]
    public void GetGameStateProperty_EmptyGameState_ReturnDefault()
    {
        // arrange
        var testGame = new Game();
        
        // act
        var numberValue = testGame.GetGameStatePropertyNumber("numberx");
        var stringValue = testGame.GetGameStatePropertyString("stringx");
        var boolValue = testGame.GetGameStatePropertyBoolean("booleanx");
        
        // assert
        Assert.Equal(0, numberValue);
        Assert.Null(stringValue);
        Assert.False(boolValue);
    }

    #endregion

    #region SetGameStateProperty

    [Fact]
    public void SetGameStateProperty_KeyExists_Overwritten()
    {
        // arrange
        var testGame = new Game()
        {
            GameState = "{\"number\":42,\"string\":\"banana\",\"boolean\":true}"
        };
        
        // act
        testGame.SetGameStatePropertyNumber("number", 43);
        testGame.SetGameStatePropertyString("string", "apple");
        testGame.SetGameStatePropertyBoolean("boolean", false);
        
        // assert
        Assert.Equal("{\"number\":43,\"string\":\"apple\",\"boolean\":false}", testGame.GameState);
    }

    [Fact]
    public void SetGameStateProperty_KeyDoesntExist_ValueAdded()
    {
        // arrange
        var testGame = new Game()
        {
            GameState = "{\"number\":42,\"string\":\"banana\",\"boolean\":true}"
        };
        
        // act
        testGame.SetGameStatePropertyBoolean("newboolean", false);
        
        // assert
        Assert.Equal("{\"number\":42,\"string\":\"banana\",\"boolean\":true,\"newboolean\":false}", testGame.GameState);
    }
    
    [Fact]
    public void SetGameStateProperty_EmptyGameState_ValueAdded()
    {
        // arrange
        var testGame = new Game();
        
        // act
        testGame.SetGameStatePropertyBoolean("newboolean", false);
        
        // assert
        Assert.Equal("{\"newboolean\":false}", testGame.GameState);
    }

    #endregion
}