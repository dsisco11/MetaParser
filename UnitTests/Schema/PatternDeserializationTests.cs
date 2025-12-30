using System.Text.Json;
using MetaParser.Schema;
using Xunit;

namespace UnitTests.Schema;

/// <summary>
/// Tests for PatternDefinition JSON deserialization.
/// </summary>
public class PatternDeserializationTests
{
    private static readonly JsonSerializerOptions s_options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters = { new PatternDefinitionConverter() }
    };

    #region Literal Pattern Tests

    [Fact]
    public void Deserialize_LiteralString_ReturnsLiteralPattern()
    {
        const string json = "\"if\"";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<LiteralPattern>(pattern);
        var literal = (LiteralPattern)pattern;
        Assert.Equal("if", literal.Value);
        Assert.Equal(PatternKind.Literal, literal.Kind);
    }

    [Fact]
    public void Deserialize_LiteralOperator_ReturnsLiteralPattern()
    {
        const string json = "\"+\"";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<LiteralPattern>(pattern);
        var literal = (LiteralPattern)pattern;
        Assert.Equal("+", literal.Value);
    }

    [Fact]
    public void Deserialize_LiteralEscapedChar_ReturnsLiteralPattern()
    {
        const string json = "\"\\n\"";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<LiteralPattern>(pattern);
        var literal = (LiteralPattern)pattern;
        Assert.Equal("\n", literal.Value);
    }

    #endregion

    #region Range Pattern Tests

    [Fact]
    public void Deserialize_RangeDigits_ReturnsRangePattern()
    {
        const string json = """{ "range": ["0", "9"] }""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<RangePattern>(pattern);
        var range = (RangePattern)pattern;
        Assert.Equal('0', range.Start);
        Assert.Equal('9', range.End);
        Assert.Equal(PatternKind.Range, range.Kind);
    }

    [Fact]
    public void Deserialize_RangeLowercase_ReturnsRangePattern()
    {
        const string json = """{ "range": ["a", "z"] }""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<RangePattern>(pattern);
        var range = (RangePattern)pattern;
        Assert.Equal('a', range.Start);
        Assert.Equal('z', range.End);
    }

    [Fact]
    public void Deserialize_RangeSingleChar_ReturnsRangePattern()
    {
        const string json = """{ "range": ["x", "x"] }""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<RangePattern>(pattern);
        var range = (RangePattern)pattern;
        Assert.Equal('x', range.Start);
        Assert.Equal('x', range.End);
    }

    [Fact]
    public void Construct_RangeInvalid_ThrowsArgumentException()
    {
        // Range where start > end should throw
        Assert.Throws<ArgumentException>(() => new RangePattern('z', 'a'));
    }

    #endregion

    #region Token Reference Pattern Tests

    [Fact]
    public void Deserialize_TokenReference_ReturnsTokenReferencePattern()
    {
        const string json = """{ "$token": "digit" }""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<TokenReferencePattern>(pattern);
        var tokenRef = (TokenReferencePattern)pattern;
        Assert.Equal("digit", tokenRef.TokenName);
        Assert.Equal(PatternKind.TokenReference, tokenRef.Kind);
    }

    [Fact]
    public void Deserialize_TokenReferenceComplex_ReturnsTokenReferencePattern()
    {
        const string json = """{ "$token": "identifier_char" }""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<TokenReferencePattern>(pattern);
        var tokenRef = (TokenReferencePattern)pattern;
        Assert.Equal("identifier_char", tokenRef.TokenName);
    }

    #endregion

    #region OneOf Pattern Tests

    [Fact]
    public void Deserialize_ArrayOfLiterals_ReturnsOneOfPattern()
    {
        const string json = """[" ", "\t", "\n"]""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<OneOfPattern>(pattern);
        var oneOf = (OneOfPattern)pattern;
        Assert.Equal(3, oneOf.Patterns.Length);
        Assert.All(oneOf.Patterns, p => Assert.IsType<LiteralPattern>(p));
        Assert.Equal(PatternKind.OneOf, oneOf.Kind);
    }

    [Fact]
    public void Deserialize_ArrayOfRanges_ReturnsOneOfPattern()
    {
        const string json = """[{ "range": ["a", "z"] }, { "range": ["A", "Z"] }]""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<OneOfPattern>(pattern);
        var oneOf = (OneOfPattern)pattern;
        Assert.Equal(2, oneOf.Patterns.Length);
        Assert.All(oneOf.Patterns, p => Assert.IsType<RangePattern>(p));
    }

    [Fact]
    public void Deserialize_MixedArray_ReturnsOneOfPattern()
    {
        const string json = """[{ "range": ["a", "z"] }, "_", { "$token": "digit" }]""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<OneOfPattern>(pattern);
        var oneOf = (OneOfPattern)pattern;
        Assert.Equal(3, oneOf.Patterns.Length);
        Assert.IsType<RangePattern>(oneOf.Patterns[0]);
        Assert.IsType<LiteralPattern>(oneOf.Patterns[1]);
        Assert.IsType<TokenReferencePattern>(oneOf.Patterns[2]);
    }

    [Fact]
    public void Deserialize_SingleElementArray_ReturnsSinglePattern()
    {
        // Optimization: single-element array unwraps to the pattern itself
        const string json = """["x"]""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<LiteralPattern>(pattern);
        var literal = (LiteralPattern)pattern;
        Assert.Equal("x", literal.Value);
    }

    [Fact]
    public void Deserialize_ArrayOfTokenRefs_ReturnsOneOfPattern()
    {
        const string json = """[{ "$token": "number" }, { "$token": "identifier" }]""";

        var pattern = JsonSerializer.Deserialize<PatternDefinition>(json, s_options);

        Assert.NotNull(pattern);
        Assert.IsType<OneOfPattern>(pattern);
        var oneOf = (OneOfPattern)pattern;
        Assert.Equal(2, oneOf.Patterns.Length);
        Assert.All(oneOf.Patterns, p => Assert.IsType<TokenReferencePattern>(p));
    }

    #endregion

    #region Full Token Definition Tests

    [Fact]
    public void Deserialize_TokenDefinition_ParsesAllPatterns()
    {
        const string json = """
        {
            "namespace": "Test",
            "tokens": {
                "identifier": {
                    "start": { "range": ["a", "z"] },
                    "consume": [{ "range": ["a", "z"] }, { "range": ["0", "9"] }, "_"]
                }
            }
        }
        """;

        var schemaOptions = new JsonSerializerOptions(s_options);
        var schema = JsonSerializer.Deserialize<SchemaDefinition>(json, schemaOptions);

        Assert.NotNull(schema);
        var token = schema.Tokens["identifier"];
        
        Assert.IsType<RangePattern>(token.Start);
        Assert.IsType<OneOfPattern>(token.Consume);
        
        var consume = (OneOfPattern)token.Consume!;
        Assert.Equal(3, consume.Patterns.Length);
    }

    #endregion
}
