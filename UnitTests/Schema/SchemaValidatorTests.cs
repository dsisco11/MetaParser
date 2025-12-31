using MetaParser.Schema;
using Xunit;

namespace UnitTests.Schema;

/// <summary>
/// Tests for SchemaValidator.
/// </summary>
public class SchemaValidatorTests
{
    #region Required Fields Validation

    [Fact]
    public void Validate_NullSchema_ReturnsError()
    {
        var result = SchemaValidator.Validate(null);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Failed to parse"));
    }

    [Fact]
    public void Validate_MissingNamespace_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "",
            Tokens = { ["test"] = new TokenDefinition { Consume = new LiteralPattern("x") } }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("namespace"));
    }

    [Fact]
    public void Validate_NoTokens_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test"
            // No tokens
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("at least one token"));
    }

    [Fact]
    public void Validate_MissingConsumePattern_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens = { ["test"] = new TokenDefinition { Consume = null } }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("'consume' pattern"));
    }

    #endregion

    #region Token Reference Validation

    [Fact]
    public void Validate_InvalidTokenReference_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["number"] = new TokenDefinition
                {
                    Consume = new TokenReferencePattern("digit") // digit not defined!
                }
            }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("undefined token 'digit'"));
    }

    [Fact]
    public void Validate_ValidTokenReference_Succeeds()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["digit"] = new TokenDefinition
                {
                    Consume = new RangePattern('0', '9')
                },
                ["number"] = new TokenDefinition
                {
                    Consume = new TokenReferencePattern("digit")
                }
            }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_SelfReference_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["recursive"] = new TokenDefinition
                {
                    Consume = new TokenReferencePattern("recursive")
                }
            }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("self-reference"));
    }

    [Fact]
    public void Validate_InvalidTokenRefInOneOf_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["value"] = new TokenDefinition
                {
                    Consume = new OneOfPattern(new PatternDefinition[]
                    {
                        new TokenReferencePattern("number"),     // not defined
                        new TokenReferencePattern("identifier")  // not defined
                    })
                }
            }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("'number'"));
        Assert.Contains(result.Errors, e => e.Contains("'identifier'"));
    }

    #endregion

    #region Trivia Validation

    [Fact]
    public void Validate_InvalidTriviaReference_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["keyword"] = new TokenDefinition { Consume = new LiteralPattern("if") }
            },
            Trivia = { "whitespace" } // whitespace not defined!
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Trivia") && e.Contains("'whitespace'"));
    }

    [Fact]
    public void Validate_ValidTriviaReference_Succeeds()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["whitespace"] = new TokenDefinition { Consume = new LiteralPattern(" ") },
                ["keyword"] = new TokenDefinition { Consume = new LiteralPattern("if") }
            },
            Trivia = { "whitespace" }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_DuplicateTrivia_ReturnsWarning()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["whitespace"] = new TokenDefinition { Consume = new LiteralPattern(" ") }
            },
            Trivia = { "whitespace", "whitespace" } // duplicate
        };

        var result = SchemaValidator.Validate(schema);

        Assert.True(result.IsValid); // Still valid, just a warning
        Assert.Contains(result.Warnings, w => w.Contains("multiple times"));
    }

    #endregion

    #region Range Validation

    [Fact]
    public void Validate_ValidRange_Succeeds()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["digit"] = new TokenDefinition
                {
                    Consume = new RangePattern('0', '9')
                }
            }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_EmptyLiteral_ReturnsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["empty"] = new TokenDefinition
                {
                    Consume = new LiteralPattern("")
                }
            }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("empty literal"));
    }

    #endregion

    #region Valid Schema Tests

    [Fact]
    public void Validate_CompleteValidSchema_Succeeds()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Calculator",
            Classname = "Lexer",
            Tokens =
            {
                ["whitespace"] = new TokenDefinition
                {
                    Consume = new OneOfPattern(new PatternDefinition[]
                    {
                        new LiteralPattern(" "),
                        new LiteralPattern("\t"),
                        new LiteralPattern("\n")
                    })
                },
                ["digit"] = new TokenDefinition
                {
                    Consume = new RangePattern('0', '9')
                },
                ["number"] = new TokenDefinition
                {
                    Consume = new TokenReferencePattern("digit")
                },
                ["plus"] = new TokenDefinition
                {
                    Consume = new LiteralPattern("+")
                },
                ["minus"] = new TokenDefinition
                {
                    Consume = new LiteralPattern("-")
                }
            },
            Trivia = { "whitespace" }
        };

        var result = SchemaValidator.Validate(schema);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        // Note: 'number' is unreferenced but references 'digit', so it gets a warning
        // This is expected behavior - real schemas would have higher-level tokens that reference 'number'
    }

    #endregion
}
