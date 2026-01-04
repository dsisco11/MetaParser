# MetaParser - AI Coding Instructions

## Project Overview

MetaParser is a **C# Roslyn Source Generator** (zero runtime dependency) that produces complete text parsers/tokenizers from `.metaparser.json` schema files. Users define tokens declaratively; the generator outputs a full parser implementation.

**Critical Architecture Principle:** All code that runs in a user's application is **GENERATED**, not referenced. The generator itself targets `netstandard2.0` (Roslyn requirement), but generated output targets `net8.0+` with C# 12 features.

## Code Generation Pipeline

The generator transforms schemas through these stages:
1. **Schema Parsing** → `SchemaDefinition` from JSON via `SchemaJsonContext`
2. **Validation** → `SchemaValidator` checks for errors (circular refs, unreachable tokens)
3. **Dependency Analysis** → `TokenDependencyAnalyzer` builds graph, computes generation order
4. **Code Emission** → Individual generators in `Generation/` folder output `.g.cs` files

### Key Generated Artifacts
| Generator | Output | Purpose |
|-----------|--------|---------|
| `GreenNodeGenerator` | `GreenNode.g.cs` | Immutable AST base class (Roslyn Red-Green pattern) |
| `GreenTokenGenerator` | `GreenToken.g.cs` | Token variants with trivia support |
| `TokenKindGenerator` | `TokenKind.g.cs` | Enum of all token types from schema |
| `LexerGenerator` | `Lexer.g.cs` | Token consumer orchestration |
| `ParserGenerator` | `Parser.g.cs` | Full parser with trivia attachment |
| `RedNodeGenerator` | `SyntaxNode.g.cs` | Lazy wrapper with parent/position |
| `RedTokenGenerator` | `SyntaxToken.g.cs` | Token wrapper with span access |

## Project Structure Patterns

```
MetaParser/           # Source generator (netstandard2.0)
├── Generation/       # Code emitters - one per generated file type
├── Schema/           # JSON schema models + validation
├── Graphs/           # Dependency graph (cycle detection, topo sort)
├── Diagnostics/      # MP0xxx error/warning definitions
└── Generator.cs      # Entry point (IIncrementalGenerator)

UnitTests/            # xUnit tests (net8.0)
├── Integration/      # Full pipeline tests (schema → compile → execute)
└── Generation/       # Individual generator output tests
```

## Code Generation Conventions

When writing or modifying generators:

1. **Use `CodeBuilder`** for indentation-aware code emission:
   ```csharp
   code.OpenBlock();     // Emits '{' and indents
   code.AppendLine("..."); 
   code.CloseBlock();    // Outdents and emits '}'
   ```

2. **Use `GeneratedFileBuilder`** for file scaffolding:
   ```csharp
   var file = new GeneratedFileBuilder(schema.Namespace)
       .WithFileName("MyType.g.cs")
       .AddUsings("System", "System.Collections.Generic");
   ```

3. **Diagnostic IDs follow ranges:**
   - `MP0000-MP0099`: Internal/general errors
   - `MP0100-MP0199`: Schema validation errors  
   - `MP0200-MP0299`: Schema validation warnings
   - `MP0300-MP0399`: Code generation errors

4. **Token priority** = definition order in schema JSON (earlier = higher priority)

5. **SIMD-optimized character matching** in generated code:
   - Use `SearchValues<char>` for multi-character sets (see `TokenConsumerGenerator.cs`)
   - Prefer `IndexOfAny`/`IndexOfAnyExcept` over character-by-character loops
   - Use `SequenceReader<char>.AdvanceWhileAnyOf(SearchValues)` for greedy consumption
   - Example pattern from generated consumers:
     ```csharp
     private static readonly SearchValues<char> s_consume = SearchValues.Create("+-*/");
     // Then: reader.AdvanceWhileAnyOf(s_consume);
     ```

6. **Trivia attachment** follows Roslyn conventions:
   - Trivia attaches to the **following** token as leading trivia
   - Exception: trailing trivia on the same line stays with the preceding token
   - Newlines split trivia ownership (newline itself is trailing, rest is leading)
   - See `TriviaAttachmentGenerator.cs` for the splitting logic

7. **Red-Green tree naming convention:**
   - Green nodes use `Green` prefix (internal, immutable, cacheable): `GreenNode`, `GreenToken`
   - Red nodes are **consumer-facing** and do NOT use `Red` prefix: `SyntaxNode`, `SyntaxToken`, `SyntaxTree`, `SyntaxTrivia`
   - The "Red/Green" terminology is an internal implementation detail, not exposed to users

## Testing Approach

### Integration Tests (compile and execute generated code)
Tests in `UnitTests/Integration/` use a compile-and-run pattern:
```csharp
// 1. Build SchemaDefinition programmatically
var schema = new SchemaDefinition { Namespace = "Test" };
schema.Tokens["number"] = new TokenDefinition { ... };

// 2. Generate all sources
var sources = GenerateAllSources(schema);

// 3. Compile to in-memory assembly
var assembly = CompileAndLoad(sources, "TestAssembly");

// 4. Use reflection to invoke generated parser
var parserType = assembly.GetType("Test.TestParser");
```

### Unit Tests (validate generator output compiles)
Tests in `UnitTests/Generation/` verify individual generators produce valid C#.

## Build & Test Commands

```powershell
# Build everything
dotnet build MetaParser.sln

# Run all tests
dotnet test UnitTests/UnitTests.csproj

# Run specific test class
dotnet test --filter "FullyQualifiedName~EndToEndIntegrationTests"
```

## Schema File Format (v2)

Schema files use `.metaparser.json` extension with this structure:
```json
{
  "$schema": "...",
  "namespace": "MyParser.Syntax",
  "classname": "MyParser",
  "tokens": {
    "whitespace": { "consume": [" ", "\t"] },
    "number": { 
      "start": { "range": ["0", "9"] },
      "consume": { "range": ["0", "9"] }
    },
    "identifier": {
      "start": { "range": ["a", "z"] },
      "consume": [{ "range": ["a", "z"] }, { "range": ["0", "9"] }]
    }
  },
  "trivia": ["whitespace"]
}
```

Pattern types: `"literal"` (string), `{ "range": ["a", "z"] }`, `{ "$token": "other" }`, or arrays combining them.

## Key Documentation

- [docs/PROJECT_OVERVIEW.md](docs/PROJECT_OVERVIEW.md) - Architecture deep-dive
- [docs/SCHEMA_V2_SPEC.md](docs/SCHEMA_V2_SPEC.md) - Complete schema reference
- [docs/GREEN_TOKEN_HIERARCHY.md](docs/GREEN_TOKEN_HIERARCHY.md) - Red-Green tree implementation
- [docs/CONSUMER_CHAIN.md](docs/CONSUMER_CHAIN.md) - Lexer/parser consumer architecture
- [project.todo](project.todo) - Implementation progress tracker
