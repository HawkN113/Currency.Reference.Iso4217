using Currency.Reference.Iso4217.Generators.Utility;

namespace Currency.Reference.Iso4217.Generators.Tests.Utility;

public class JsonParserTests
{
    [Fact]
    public void ExtractPublishedDate_ShouldReturnValue_WhenValidJson()
    {
        // Arrange
        var json = """{ "_Pblshd": "2025-10-28" }""";

        // Act
        var result = JsonParser.ExtractPublishedDate(json);

        // Assert
        Assert.Equal("2025-10-28", result);
    }

    [Fact]
    public void ExtractPublishedDate_ShouldThrow_WhenKeyMissing()
    {
        // Arrange
        var json = """{ "Other": "value" }""";

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => JsonParser.ExtractPublishedDate(json));
        Assert.Contains("Published date", ex.Message);
    }

    [Fact]
    public void ExtractPublishedDate_ShouldThrow_WhenJsonIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => JsonParser.ExtractPublishedDate(null!));
    }

    [Fact]
    public void ExtractArray_ShouldReturnArrayContent_WhenValidJson()
    {
        // Arrange
        var json = """{ "Data": [ { "Value": 1 }, { "Value": 2 } ] }""";

        // Act
        var result = JsonParser.ExtractArray(json, "Data");

        // Assert
        Assert.Contains(@"""Value"": 1", result);
        Assert.Contains(@"""Value"": 2", result);
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenKeyNotFound()
    {
        var json = """{ "Other": [] }""";
        var ex = Assert.Throws<InvalidOperationException>(() => JsonParser.ExtractArray(json, "Data"));
        Assert.Contains("required in the JSON content", ex.Message);
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenArrayIsMalformed()
    {
        var json = """{ "Data": [ { "A": 1 }"""; // нет закрывающей скобки
        var ex = Assert.Throws<InvalidOperationException>(() => JsonParser.ExtractArray(json, "Data"));
        Assert.Contains("Cannot locate end", ex.Message);
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenJsonIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => JsonParser.ExtractArray(null!, "Key"));
    }

    [Fact]
    public void ExtractArray_ShouldThrow_WhenKeyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => JsonParser.ExtractArray("{}", null!));
    }

    [Fact]
    public void Extract_ShouldReturnString_WhenKeyIsString()
    {
        var json = """{ "Name": "Euro" }""";
        var result = JsonParser.Extract(json, "Name");
        Assert.Equal("Euro", result);
    }

    [Fact]
    public void Extract_ShouldReturnNumber_WhenKeyIsNumeric()
    {
        var json = """{ "Code": 840 }""";
        var result = JsonParser.Extract(json, "Code");
        Assert.Equal("840", result);
    }

    [Fact]
    public void Extract_ShouldReturnNull_WhenKeyNotFound()
    {
        var json = """{ "X": 1 }""";
        var result = JsonParser.Extract(json, "Y");
        Assert.Null(result);
    }

    [Fact]
    public void Extract_ShouldReturnNormalizedString_WithSingleQuotes()
    {
        var json = """{ "Text": "He said \"Hello\"" }""";
        var result = JsonParser.Extract(json, "Text");
        Assert.Equal("He said 'Hello'", result);
    }

    [Fact]
    public void HandleEscapeChar_ShouldToggleStringState()
    {
        bool inString = false, escape = false;

        Assert.True(JsonParser.HandleEscapeChar('"', ref inString, ref escape));
        Assert.True(inString);
        Assert.True(JsonParser.HandleEscapeChar('"', ref inString, ref escape));
        Assert.False(inString);
    }

    [Fact]
    public void HandleEscapeChar_ShouldHandleBackslash()
    {
        bool inString = false, escape = false;
        Assert.True(JsonParser.HandleEscapeChar('\\', ref inString, ref escape));
        Assert.True(escape);
    }

    [Fact]
    public void HandleEscapeChar_ShouldReturnFalseForOtherChars()
    {
        bool inString = false, escape = false;
        var result = JsonParser.HandleEscapeChar('x', ref inString, ref escape);
        Assert.False(result);
    }
}