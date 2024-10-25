using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using MetaParser.Core;

namespace MetaParser.Builders.Core;
internal class MethodCodeBuilder : CodeBuilder
{
    private readonly NameSyntax _name;
    private readonly TypeSyntax _returnType;
    private readonly SyntaxTokenList? _modifiers;
    private readonly ArgumentListSyntax _arguments;

    public MethodCodeBuilder(IEnumerable<SyntaxToken> modifiers, TypeSyntax returnType, string name, ArgumentListSyntax arguments)
    {
        _modifiers = SyntaxFactory.TokenList(modifiers);
        _name = SyntaxFactory.ParseName(name);
        _returnType = returnType;
        _arguments = arguments;
    }

    protected override void WriteTo(CodeGenContext context)
    {

        if (_modifiers is not null)
        {
            foreach (var mod in _modifiers)
            {
                mod.WriteTo(context.Writer);
            }

            if (_modifiers?.Count > 0)
            {
                context.Writer.Write(" ");
            }
        }

        _returnType.WriteTo(context.Writer);
        context.Writer.Write(" ");
        _name.WriteTo(context.Writer);
        context.Writer.Write("(");
        _arguments.WriteTo(context.Writer);
        context.Writer.Write(")");
        context.Writer.WriteLine();
        context.Writer.WriteLine("{");
        context.Writer.Indent++;

        GenerateInnerItems(context with { State = context.State with { ActiveBuffer = 0 } });

        context.Writer.Indent--;
        context.Writer.WriteLine("}");
    }
}
