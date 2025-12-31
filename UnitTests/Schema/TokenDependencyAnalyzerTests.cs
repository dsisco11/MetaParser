using MetaParser.Schema;
using Xunit;

namespace UnitTests.Schema;

/// <summary>
/// Unit tests for TokenDependencyAnalyzer.
/// </summary>
public class TokenDependencyAnalyzerTests
{
    [Fact]
    public void Analyze_EmptySchema_CreatesEmptyGraph()
    {
        var schema = new SchemaDefinition { Namespace = "Test" };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        analyzer.Analyze();
        
        Assert.Equal(0, analyzer.Graph.NodeCount);
    }
    
    [Fact]
    public void Analyze_SimpleTokens_NoEdges()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["number"] = new TokenDefinition { Consume = new RangePattern('0', '9') },
                ["letter"] = new TokenDefinition { Consume = new RangePattern('a', 'z') }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        analyzer.Analyze();
        
        Assert.Equal(2, analyzer.Graph.NodeCount);
        Assert.Equal(0, analyzer.Graph.EdgeCount);
    }
    
    [Fact]
    public void Analyze_TokenReference_CreatesEdge()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["digit"] = new TokenDefinition { Consume = new RangePattern('0', '9') },
                ["number"] = new TokenDefinition { Consume = new TokenReferencePattern("digit") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        analyzer.Analyze();
        
        Assert.Equal(2, analyzer.Graph.NodeCount);
        Assert.Equal(1, analyzer.Graph.EdgeCount);
        Assert.True(analyzer.Graph.HasEdge("digit", "number"));
    }
    
    [Fact]
    public void HasCircularDependencies_NoCycles_ReturnsFalse()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new LiteralPattern("a") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("a") },
                ["c"] = new TokenDefinition { Consume = new TokenReferencePattern("b") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        Assert.False(analyzer.HasCircularDependencies());
    }
    
    [Fact]
    public void HasCircularDependencies_SimpleCycle_ReturnsTrue()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new TokenReferencePattern("b") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("c") },
                ["c"] = new TokenDefinition { Consume = new TokenReferencePattern("a") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        Assert.True(analyzer.HasCircularDependencies());
    }
    
    [Fact]
    public void FindFirstCycle_NoCycles_ReturnsNull()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new LiteralPattern("a") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("a") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        Assert.Null(analyzer.FindFirstCycle());
    }
    
    [Fact]
    public void FindFirstCycle_WithCycle_ReturnsCycleNodes()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new TokenReferencePattern("b") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("a") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        var cycle = analyzer.FindFirstCycle();
        
        Assert.NotNull(cycle);
        Assert.True(cycle.Count >= 2);
    }
    
    [Fact]
    public void GetCyclicTokens_NoCycles_ReturnsEmpty()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new LiteralPattern("a") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        Assert.Empty(analyzer.GetCyclicTokens());
    }
    
    [Fact]
    public void GetCyclicTokens_WithCycle_ReturnsAllCyclicTokens()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new TokenReferencePattern("b") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("c") },
                ["c"] = new TokenDefinition { Consume = new TokenReferencePattern("a") },
                ["d"] = new TokenDefinition { Consume = new LiteralPattern("d") } // Not in cycle
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        var cyclic = analyzer.GetCyclicTokens();
        
        Assert.Equal(3, cyclic.Count);
        Assert.Contains("a", cyclic);
        Assert.Contains("b", cyclic);
        Assert.Contains("c", cyclic);
    }
    
    [Fact]
    public void GetUnreferencedTokens_AllIndependent_ReturnsAll()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new LiteralPattern("a") },
                ["b"] = new TokenDefinition { Consume = new LiteralPattern("b") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        var unreferenced = analyzer.GetUnreferencedTokens();
        
        Assert.Equal(2, unreferenced.Count);
    }
    
    [Fact]
    public void GetUnreferencedTokens_WithReferences_ReturnsOnlyUnreferenced()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["digit"] = new TokenDefinition { Consume = new RangePattern('0', '9') },
                ["number"] = new TokenDefinition { Consume = new TokenReferencePattern("digit") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        var unreferenced = analyzer.GetUnreferencedTokens();
        
        // "digit" is referenced by "number" (number's pattern contains $digit)
        // "number" is NOT referenced by anything
        Assert.Single(unreferenced);
        Assert.Contains("number", unreferenced);
    }
    
    [Fact]
    public void GetGenerationOrder_NoCycles_ReturnsDependencyOrder()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["digit"] = new TokenDefinition { Consume = new RangePattern('0', '9') },
                ["number"] = new TokenDefinition { Consume = new TokenReferencePattern("digit") },
                ["expr"] = new TokenDefinition { Consume = new TokenReferencePattern("number") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        var order = analyzer.GetGenerationOrder();
        
        Assert.NotNull(order);
        Assert.Equal(3, order.Count);
        
        // digit should come before number, number before expr
        var indexDigit = order.ToList().IndexOf("digit");
        var indexNumber = order.ToList().IndexOf("number");
        var indexExpr = order.ToList().IndexOf("expr");
        
        Assert.True(indexDigit < indexNumber);
        Assert.True(indexNumber < indexExpr);
    }
    
    [Fact]
    public void GetGenerationOrder_WithCycles_ReturnsNull()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new TokenReferencePattern("b") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("a") }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        Assert.Null(analyzer.GetGenerationOrder());
    }
    
    [Fact]
    public void Analyze_OneOfPattern_CollectsAllReferences()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["digit"] = new TokenDefinition { Consume = new RangePattern('0', '9') },
                ["letter"] = new TokenDefinition { Consume = new RangePattern('a', 'z') },
                ["alphanumeric"] = new TokenDefinition
                {
                    Consume = new OneOfPattern(new PatternDefinition[]
                    {
                        new TokenReferencePattern("digit"),
                        new TokenReferencePattern("letter")
                    })
                }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        analyzer.Analyze();
        
        Assert.True(analyzer.Graph.HasEdge("digit", "alphanumeric"));
        Assert.True(analyzer.Graph.HasEdge("letter", "alphanumeric"));
    }
    
    [Fact]
    public void Analyze_AllPatternTypes_CollectsReferences()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["quote"] = new TokenDefinition { Consume = new LiteralPattern("\"") },
                ["escape"] = new TokenDefinition { Consume = new LiteralPattern("\\") },
                ["char"] = new TokenDefinition { Consume = new RangePattern(' ', '~') },
                ["string"] = new TokenDefinition
                {
                    Start = new TokenReferencePattern("quote"),
                    Consume = new TokenReferencePattern("char"),
                    Stop = new TokenReferencePattern("quote"),
                    Escape = new TokenReferencePattern("escape")
                }
            }
        };
        var analyzer = new TokenDependencyAnalyzer(schema);
        
        analyzer.Analyze();
        
        Assert.True(analyzer.Graph.HasEdge("quote", "string"));
        Assert.True(analyzer.Graph.HasEdge("char", "string"));
        Assert.True(analyzer.Graph.HasEdge("escape", "string"));
    }
}

/// <summary>
/// Integration tests for SchemaValidator circular dependency detection.
/// </summary>
public class SchemaValidatorCircularDependencyTests
{
    [Fact]
    public void Validate_NoCycles_NoErrors()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["digit"] = new TokenDefinition { Consume = new RangePattern('0', '9') },
                ["number"] = new TokenDefinition { Consume = new TokenReferencePattern("digit") }
            }
        };
        
        var result = SchemaValidator.Validate(schema);
        
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void Validate_SimpleCycle_ReportsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new TokenReferencePattern("b") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("a") }
            }
        };
        
        var result = SchemaValidator.Validate(schema);
        
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Circular dependency"));
    }
    
    [Fact]
    public void Validate_ThreeNodeCycle_ReportsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new TokenReferencePattern("b") },
                ["b"] = new TokenDefinition { Consume = new TokenReferencePattern("c") },
                ["c"] = new TokenDefinition { Consume = new TokenReferencePattern("a") }
            }
        };
        
        var result = SchemaValidator.Validate(schema);
        
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Circular dependency"));
    }
    
    [Fact]
    public void Validate_SelfReference_ReportsError()
    {
        var schema = new SchemaDefinition
        {
            Namespace = "Test",
            Tokens =
            {
                ["a"] = new TokenDefinition { Consume = new TokenReferencePattern("a") }
            }
        };
        
        var result = SchemaValidator.Validate(schema);
        
        Assert.False(result.IsValid);
        // Self-reference is caught as a specific error in ValidatePatternReferences
        Assert.Contains(result.Errors, e => e.Contains("self-reference"));
    }
}
