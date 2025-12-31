using MetaParser.Diagnostics;
using Microsoft.CodeAnalysis;
using Xunit;

namespace UnitTests.Diagnostics;

/// <summary>
/// Tests for DiagnosticDescriptors.
/// </summary>
public class DiagnosticDescriptorTests
{
    [Fact]
    public void Info_HasCorrectId()
    {
        Assert.Equal("MP0000", DiagnosticDescriptors.Info.Id);
        Assert.Equal(DiagnosticSeverity.Info, DiagnosticDescriptors.Info.DefaultSeverity);
    }
    
    [Fact]
    public void JsonParseError_HasCorrectId()
    {
        Assert.Equal("MP0001", DiagnosticDescriptors.JsonParseError.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.JsonParseError.DefaultSeverity);
    }
    
    [Fact]
    public void SchemaValidationError_HasCorrectId()
    {
        Assert.Equal("MP0100", DiagnosticDescriptors.SchemaValidationError.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.SchemaValidationError.DefaultSeverity);
    }
    
    [Fact]
    public void MissingNamespace_HasCorrectId()
    {
        Assert.Equal("MP0101", DiagnosticDescriptors.MissingNamespace.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.MissingNamespace.DefaultSeverity);
    }
    
    [Fact]
    public void MissingTokens_HasCorrectId()
    {
        Assert.Equal("MP0102", DiagnosticDescriptors.MissingTokens.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.MissingTokens.DefaultSeverity);
    }
    
    [Fact]
    public void MissingConsumePattern_HasCorrectId()
    {
        Assert.Equal("MP0103", DiagnosticDescriptors.MissingConsumePattern.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.MissingConsumePattern.DefaultSeverity);
    }
    
    [Fact]
    public void UndefinedTokenReference_HasCorrectId()
    {
        Assert.Equal("MP0104", DiagnosticDescriptors.UndefinedTokenReference.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.UndefinedTokenReference.DefaultSeverity);
    }
    
    [Fact]
    public void SelfReference_HasCorrectId()
    {
        Assert.Equal("MP0105", DiagnosticDescriptors.SelfReference.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.SelfReference.DefaultSeverity);
    }
    
    [Fact]
    public void CircularDependency_HasCorrectId()
    {
        Assert.Equal("MP0106", DiagnosticDescriptors.CircularDependency.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.CircularDependency.DefaultSeverity);
    }
    
    [Fact]
    public void InvalidCharacterRange_HasCorrectId()
    {
        Assert.Equal("MP0107", DiagnosticDescriptors.InvalidCharacterRange.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.InvalidCharacterRange.DefaultSeverity);
    }
    
    [Fact]
    public void EmptyLiteral_HasCorrectId()
    {
        Assert.Equal("MP0108", DiagnosticDescriptors.EmptyLiteral.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.EmptyLiteral.DefaultSeverity);
    }
    
    [Fact]
    public void UndefinedTriviaReference_HasCorrectId()
    {
        Assert.Equal("MP0109", DiagnosticDescriptors.UndefinedTriviaReference.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.UndefinedTriviaReference.DefaultSeverity);
    }
    
    [Fact]
    public void DuplicateTrivia_HasCorrectId()
    {
        Assert.Equal("MP0110", DiagnosticDescriptors.DuplicateTrivia.Id);
        Assert.Equal(DiagnosticSeverity.Warning, DiagnosticDescriptors.DuplicateTrivia.DefaultSeverity);
    }
    
    [Fact]
    public void SchemaValidationWarning_HasCorrectId()
    {
        Assert.Equal("MP0200", DiagnosticDescriptors.SchemaValidationWarning.Id);
        Assert.Equal(DiagnosticSeverity.Warning, DiagnosticDescriptors.SchemaValidationWarning.DefaultSeverity);
    }
    
    [Fact]
    public void UnreferencedToken_HasCorrectId()
    {
        Assert.Equal("MP0201", DiagnosticDescriptors.UnreferencedToken.Id);
        Assert.Equal(DiagnosticSeverity.Warning, DiagnosticDescriptors.UnreferencedToken.DefaultSeverity);
    }
    
    [Fact]
    public void CodeGenerationFailed_HasCorrectId()
    {
        Assert.Equal("MP0300", DiagnosticDescriptors.CodeGenerationFailed.Id);
        Assert.Equal(DiagnosticSeverity.Error, DiagnosticDescriptors.CodeGenerationFailed.DefaultSeverity);
    }
    
    [Fact]
    public void AllDescriptors_AreEnabledByDefault()
    {
        Assert.True(DiagnosticDescriptors.Info.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.JsonParseError.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.InternalError.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.SchemaValidationError.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.MissingNamespace.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.MissingTokens.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.MissingConsumePattern.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.UndefinedTokenReference.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.SelfReference.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.CircularDependency.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.InvalidCharacterRange.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.EmptyLiteral.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.UndefinedTriviaReference.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.DuplicateTrivia.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.SchemaValidationWarning.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.UnreferencedToken.IsEnabledByDefault);
        Assert.True(DiagnosticDescriptors.CodeGenerationFailed.IsEnabledByDefault);
    }
    
    [Fact]
    public void AllDescriptors_HaveUniqueIds()
    {
        var ids = new HashSet<string>
        {
            DiagnosticDescriptors.Info.Id,
            DiagnosticDescriptors.JsonParseError.Id,
            DiagnosticDescriptors.InternalError.Id,
            DiagnosticDescriptors.SchemaValidationError.Id,
            DiagnosticDescriptors.MissingNamespace.Id,
            DiagnosticDescriptors.MissingTokens.Id,
            DiagnosticDescriptors.MissingConsumePattern.Id,
            DiagnosticDescriptors.UndefinedTokenReference.Id,
            DiagnosticDescriptors.SelfReference.Id,
            DiagnosticDescriptors.CircularDependency.Id,
            DiagnosticDescriptors.InvalidCharacterRange.Id,
            DiagnosticDescriptors.EmptyLiteral.Id,
            DiagnosticDescriptors.UndefinedTriviaReference.Id,
            DiagnosticDescriptors.DuplicateTrivia.Id,
            DiagnosticDescriptors.SchemaValidationWarning.Id,
            DiagnosticDescriptors.UnreferencedToken.Id,
            DiagnosticDescriptors.CodeGenerationFailed.Id
        };
        
        // If any IDs are duplicate, the set will have fewer elements
        Assert.Equal(17, ids.Count);
    }
    
    [Fact]
    public void ErrorDescriptors_HaveCorrectCategory()
    {
        Assert.Equal("MetaParser.Schema", DiagnosticDescriptors.SchemaValidationError.Category);
        Assert.Equal("MetaParser.Schema", DiagnosticDescriptors.MissingNamespace.Category);
        Assert.Equal("MetaParser.Schema", DiagnosticDescriptors.CircularDependency.Category);
        Assert.Equal("MetaParser.CodeGen", DiagnosticDescriptors.CodeGenerationFailed.Category);
        Assert.Equal("MetaParser", DiagnosticDescriptors.Info.Category);
    }
}

/// <summary>
/// Tests for DiagnosticReporter.
/// </summary>
public class DiagnosticReporterTests
{
    [Fact]
    public void CreateDiagnostic_WithDescriptor_CreatesDiagnostic()
    {
        var diagnostic = DiagnosticReporter.CreateDiagnostic(
            DiagnosticDescriptors.SchemaValidationError,
            "Test error message");
        
        Assert.Equal("MP0100", diagnostic.Id);
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Contains("Test error message", diagnostic.GetMessage());
    }
    
    [Fact]
    public void CreateDiagnostic_CircularDependency_FormatsMessage()
    {
        var diagnostic = DiagnosticReporter.CreateDiagnostic(
            DiagnosticDescriptors.CircularDependency,
            "A → B → C → A");
        
        Assert.Equal("MP0106", diagnostic.Id);
        Assert.Contains("A → B → C → A", diagnostic.GetMessage());
    }
    
    [Fact]
    public void CreateDiagnostic_UndefinedTokenReference_FormatsMessage()
    {
        var diagnostic = DiagnosticReporter.CreateDiagnostic(
            DiagnosticDescriptors.UndefinedTokenReference,
            "myToken", "undefinedToken", "consume");
        
        Assert.Equal("MP0104", diagnostic.Id);
        Assert.Contains("myToken", diagnostic.GetMessage());
        Assert.Contains("undefinedToken", diagnostic.GetMessage());
        Assert.Contains("consume", diagnostic.GetMessage());
    }
    
    [Fact]
    public void CreateDiagnostic_NoLocation_HasLocationNone()
    {
        var diagnostic = DiagnosticReporter.CreateDiagnostic(
            DiagnosticDescriptors.Info,
            "Test message");
        
        Assert.Equal(Location.None, diagnostic.Location);
    }
}
