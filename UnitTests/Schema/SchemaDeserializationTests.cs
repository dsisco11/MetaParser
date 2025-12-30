using System.Text.Json;
using MetaParser.Schema;
using Xunit;

namespace UnitTests.Schema;

/// <summary>
/// Tests for SchemaDefinition JSON deserialization.
/// </summary>
public class SchemaDeserializationTests
{
    private static readonly JsonSerializerOptions s_options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    [Fact]
    public void Deserialize_MinimalSchema_Succeeds()
    {
        const string json = """
        {
            "namespace": "TestParser",
            "tokens": {
                "digit": { "consume": { "range": ["0", "9"] } }
            }
        }
        """;

        var schema = JsonSerializer.Deserialize<SchemaDefinition>(json, s_options);

        Assert.NotNull(schema);
        Assert.Equal("TestParser", schema.Namespace);
        Assert.Equal("Parser", schema.Classname); // Default
        Assert.Single(schema.Tokens);
        Assert.True(schema.Tokens.ContainsKey("digit"));
    }

    [Fact]
    public void Deserialize_FullSchema_Succeeds()
    {
        const string json = """
        {
            "$schema": "https://example.com/schema.json",
            "namespace": "MyParser",
            "classname": "Lexer",
            "tokens": {
                "whitespace": { "consume": [" ", "\t", "\n"] },
                "keyword_if": { "consume": "if" }
            },
            "trivia": ["whitespace"]
        }
        """;

        var schema = JsonSerializer.Deserialize<SchemaDefinition>(json, s_options);

        Assert.NotNull(schema);
        Assert.Equal("MyParser", schema.Namespace);
        Assert.Equal("Lexer", schema.Classname);
        Assert.Equal(2, schema.Tokens.Count);
        Assert.Single(schema.Trivia);
        Assert.Equal("whitespace", schema.Trivia[0]);
    }

    [Fact]
    public void Deserialize_TokenWithAllPatterns_Succeeds()
    {
        const string json = """
        {
            "namespace": "Test",
            "tokens": {
                "string": {
                    "start": "\"",
                    "consume": { "range": [" ", "~"] },
                    "stop": "\"",
                    "escape": "\\"
                }
            }
        }
        """;

        var schema = JsonSerializer.Deserialize<SchemaDefinition>(json, s_options);

        Assert.NotNull(schema);
        var token = schema.Tokens["string"];
        Assert.NotNull(token.Start);
        Assert.NotNull(token.Consume);
        Assert.NotNull(token.Stop);
        Assert.NotNull(token.Escape);
    }

    [Fact]
    public void GetTokenPriority_ReturnsCorrectIndex()
    {
        const string json = """
        {
            "namespace": "Test",
            "tokens": {
                "first": { "consume": "a" },
                "second": { "consume": "b" },
                "third": { "consume": "c" }
            }
        }
        """;

        var schema = JsonSerializer.Deserialize<SchemaDefinition>(json, s_options);

        Assert.NotNull(schema);
        Assert.Equal(0, schema.GetTokenPriority("first"));
        Assert.Equal(1, schema.GetTokenPriority("second"));
        Assert.Equal(2, schema.GetTokenPriority("third"));
        Assert.Equal(-1, schema.GetTokenPriority("nonexistent"));
    }

    [Fact]
    public void IsTrivia_ReturnsCorrectValue()
    {
        const string json = """
        {
            "namespace": "Test",
            "tokens": {
                "whitespace": { "consume": " " },
                "keyword": { "consume": "if" }
            },
            "trivia": ["whitespace"]
        }
        """;

        var schema = JsonSerializer.Deserialize<SchemaDefinition>(json, s_options);

        Assert.NotNull(schema);
        Assert.True(schema.IsTrivia("whitespace"));
        Assert.False(schema.IsTrivia("keyword"));
    }
}
