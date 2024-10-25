using MetaParser.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Core;

internal class SwitchBlockCodeBuilder : CodeBuilder
{
    private readonly SyntaxTokenList _statement;

    public SwitchBlockCodeBuilder(string statement)
    {
        _statement = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens(statement));
    }

    protected override void WriteTo(CodeGenContext context)
    {
        context.Writer.Write("switch (");

        foreach(var token in _statement)
        {
            context.Writer.Write(token);
        }

        context.Writer.Write(")");
        context.Writer.WriteLine("{");
        context.Writer.Indent++;

        GenerateInnerItems(context);

        context.Writer.Indent--;
        context.Writer.WriteLine("}");
    }
}