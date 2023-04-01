using MetaParser.Builders.Interfaces;
using MetaParser.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ParsingLogic : MetaCodeBuilder
{
    protected override void Write(ParserContext context)
    {
        const string VarNameValueTokensArray = "tokensArray";
        const string VarNameValueTokensBuffer = "tokensBuffer";
        var writer = context.Writer;

        var tyInputBuffer = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{context.Config.InputType}>");
        var tyTokenList = SyntaxFactory.ParseTypeName($"{TokenRecordTypeName}[]");
        

        writer.Write("public ");
        writer.WriteLine($"{tyTokenList} Parse({tyInputBuffer} {context.State.ActiveBufferName})");
        writer.WriteLine("{");
        writer.Indent++;
        // constant-tokens
        writer.WriteLine($"var {VarNameValueTokensArray} = {LogicLexerTokenProcessor.FunctionName}({context.State.ActiveBufferName});");
        // compound-tokens
        writer.WriteLine($"var {VarNameValueTokensBuffer} = new {ReadOnlyMemory}<{TokenValueStructName}>( {VarNameValueTokensArray} );");
        writer.WriteLine($"return {LogicSyntaxTokenProcessor.FunctionName}({VarNameValueTokensBuffer});");
        writer.WriteLine();

        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
