using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;

namespace MetaParser.Builders.Core;
internal class FunctionDefinition : MetaCodeBuilder
{
    #region Properties
    public SyntaxTokenList? Modifiers { get; }
    public TypeSyntax ReturnType { get; }
    public NameSyntax Name { get; }
    public ArgumentListSyntax Arguments { get; }
    #endregion

    #region Constructors
    public FunctionDefinition(TypeSyntax returnType, string name, ArgumentListSyntax arguments)
    {
        ReturnType = returnType;
        Name = SyntaxFactory.ParseName(name);
        Arguments = arguments;
    }

    public FunctionDefinition(IEnumerable<SyntaxToken> modifiers, TypeSyntax returnType, string name, ArgumentListSyntax arguments)
    {
        Modifiers = SyntaxFactory.TokenList(modifiers);
        ReturnType = returnType;
        Name = SyntaxFactory.ParseName(name);
        Arguments = arguments;
    }

    #endregion

    protected override void Write(MetaParserContext context)
    {
        var writer = context.writer;
        if (Modifiers is not null)
        {
            foreach (var mod in Modifiers)
            {
                mod.WriteTo(writer);
            }

            if (Modifiers?.Count > 0)
            {
                writer.Write(" ");
            }
        }

        ReturnType.WriteTo(writer);
        writer.Write(" ");
        Name.WriteTo(writer);
        writer.Write("(");
        Arguments.WriteTo(writer);
        writer.Write(")");
        writer.WriteLine();
        writer.WriteLine("{");
        writer.Indent++;

        base.WriteContent(context);

        writer.Indent--;
        writer.WriteLine("}");
    }
}
