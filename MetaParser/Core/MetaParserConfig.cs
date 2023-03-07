using MetaParser.Builders.Core;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MetaParser.Core;

internal record MetaParserConfig
{
    public string BaseFileName { get; set; } = string.Empty;
    public string? ClassName { get; set; } = "Parser";
    public TypeSyntax IdType { get; set; } = SyntaxFactory.ParseTypeName("int");
    public TypeSyntax InputType { get; set; } = SyntaxFactory.ParseTypeName("char");
    public string Namespace { get; set; } = string.Empty;
    public string? ParserType { get; set; }
    public readonly CodeBuilderFactory CodeFactory;

    public MetaParserConfig()
    {
        CodeFactory = new CodeBuilderFactory(this);
    }
}