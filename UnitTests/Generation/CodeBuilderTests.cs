using MetaParser.Generation;
using Xunit;

namespace UnitTests.Generation;

/// <summary>
/// Tests for CodeBuilder.
/// </summary>
public class CodeBuilderTests
{
    [Fact]
    public void AppendLine_AddsText()
    {
        var builder = new CodeBuilder();
        builder.AppendLine("hello");
        
        Assert.Equal("hello\r\n", builder.ToString());
    }

    [Fact]
    public void Indent_AddsIndentation()
    {
        var builder = new CodeBuilder();
        builder.Indent();
        builder.AppendLine("indented");
        
        Assert.Equal("    indented\r\n", builder.ToString());
    }

    [Fact]
    public void Outdent_RemovesIndentation()
    {
        var builder = new CodeBuilder();
        builder.Indent();
        builder.Indent();
        builder.Outdent();
        builder.AppendLine("one level");
        
        Assert.Equal("    one level\r\n", builder.ToString());
    }

    [Fact]
    public void OpenBlock_IncreasesIndent()
    {
        var builder = new CodeBuilder();
        builder.AppendLine("class Foo");
        builder.OpenBlock();
        builder.AppendLine("int x;");
        builder.CloseBlock();
        
        var result = builder.ToString();
        Assert.Contains("class Foo", result);
        Assert.Contains("{", result);
        Assert.Contains("    int x;", result);
        Assert.Contains("}", result);
    }

    [Fact]
    public void WithBlock_ExecutesActionIndented()
    {
        var builder = new CodeBuilder();
        builder.AppendLine("if (true)");
        builder.WithBlock(b => b.AppendLine("DoSomething();"));
        
        var result = builder.ToString();
        Assert.Contains("if (true)", result);
        Assert.Contains("    DoSomething();", result);
    }

    [Fact]
    public void AppendSummary_AddsXmlDoc()
    {
        var builder = new CodeBuilder();
        builder.AppendSummary("This is a summary.");
        
        var result = builder.ToString();
        Assert.Contains("/// <summary>", result);
        Assert.Contains("/// This is a summary.", result);
        Assert.Contains("/// </summary>", result);
    }

    [Fact]
    public void AppendSummaryLine_AddsSingleLineDoc()
    {
        var builder = new CodeBuilder();
        builder.AppendSummaryLine("Short summary.");
        
        Assert.Contains("/// <summary>Short summary.</summary>", builder.ToString());
    }

    [Fact]
    public void AppendRegion_AddsRegionDirective()
    {
        var builder = new CodeBuilder();
        builder.AppendRegion("MyRegion");
        builder.AppendLine("// code");
        builder.AppendEndRegion();
        
        var result = builder.ToString();
        Assert.Contains("#region MyRegion", result);
        Assert.Contains("#endregion", result);
    }

    [Fact]
    public void Clear_ResetsBuilder()
    {
        var builder = new CodeBuilder();
        builder.Indent();
        builder.AppendLine("hello");
        builder.Clear();
        builder.AppendLine("world");
        
        Assert.Equal("world\r\n", builder.ToString());
    }

    [Fact]
    public void CustomIndentString_Works()
    {
        var builder = new CodeBuilder("\t");
        builder.Indent();
        builder.AppendLine("tabbed");
        
        Assert.Equal("\ttabbed\r\n", builder.ToString());
    }
}
