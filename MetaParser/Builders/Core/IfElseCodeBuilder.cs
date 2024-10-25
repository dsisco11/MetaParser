using MetaParser.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.Collections;

namespace MetaParser.Builders.Core;

internal class IfElseCodeBuilder : CodeBuilder
{
    private readonly SyntaxTokenList _statement;
    private readonly CodeBuilder _trueBuilder;
    private readonly CodeBuilder? _falseBuilder;

    public IfElseCodeBuilder(string statement, CodeBuilder trueBuilder, CodeBuilder? falseBuilder)
    {
        _statement = SyntaxFactory.TokenList(SyntaxFactory.ParseTokens(statement));
        _trueBuilder = trueBuilder;
        _falseBuilder = falseBuilder;
    }
    protected override void WriteTo(CodeGenContext context)
    {
        context.Writer.Write($"if (");

        foreach (var statement in _statement)
        {
            statement.WriteTo(context.Writer);
        }

        context.Writer.Write($")");
        context.Writer.WriteLine("{");
        context.Writer.Indent++;

        _trueBuilder.GenerateCode(context);

        context.Writer.Indent--;
        context.Writer.WriteLine("}");

        if (_falseBuilder is not null)
        {
            context.Writer.WriteLine("else // Condition evaluated to false");
            context.Writer.WriteLine("{");
            context.Writer.Indent++;

            _falseBuilder.GenerateCode(context);

            context.Writer.Indent--;
            context.Writer.WriteLine("}");
        }
    }
}
