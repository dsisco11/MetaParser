using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using MetaParser.Parsing.Constructs.Core;

using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ParsingLogic : MetaCodeBuilder
{
    protected override void Write(MetaParserContext context)
    {
        const string VarNameValueTokensArray = "tokensArray";
        const string VarNameValueTokensBuffer = "tokensBuffer";
        var writer = context.writer;

        var tyInputBuffer = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{context.Config.InputType}>");
        var tyTokenList = SyntaxFactory.ParseTypeName($"{TokenRecordTypeName}[]");
        

        writer.Write("public ");
        writer.WriteLine($"{tyTokenList} Parse({tyInputBuffer} {VarNameBufferMajor})");
        writer.WriteLine("{");
        writer.Indent++;
        // constant-tokens
        writer.WriteLine($"var {VarNameValueTokensArray} = {ConstantTokenStage.FunctionName}({VarNameBufferMajor});");
        // compound-tokens
        writer.WriteLine($"var {VarNameValueTokensBuffer} = new {ReadOnlyMemory}<{TokenValueStructName}>( {VarNameValueTokensArray} );");
        writer.WriteLine($"return {CompoundTokenStage.FunctionName}({VarNameValueTokensBuffer});");
        writer.WriteLine();

        writer.Indent--;
        writer.WriteLine("}");// end function
    }
}
