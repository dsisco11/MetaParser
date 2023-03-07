using MetaParser.Builders.Interfaces;
using MetaParser.Core;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions;
using static CodeCommon;

internal class ParsingLogic : IMetaCodeBuilder
{
    public void WriteTo(MetaParserContext context)
    {
        const string VarNameValueTokensArray = "tokensArray";
        const string VarNameValueTokensBuffer = "tokensBuffer";
        var wr = context.writer;

        var tyInputBuffer = SyntaxFactory.ParseTypeName($"{ReadOnlyMemory}<{context.Config.InputType}>");
        var tyTokenList = SyntaxFactory.ParseTypeName($"{TokenRecordTypeName}[]");
        

        wr.Write("public ");
        wr.WriteLine($"{tyTokenList} Parse({tyInputBuffer} {VarNameBufferMajor})");
        wr.WriteLine("{");
        wr.Indent++;
        // constant-tokens
        wr.WriteLine($"var {VarNameValueTokensArray} = {ConstantTokenStage.FunctionName}({VarNameBufferMajor});");
        // compound-tokens
        wr.WriteLine($"var {VarNameValueTokensBuffer} = new {ReadOnlyMemory}<{TokenValueStructName}>( {VarNameValueTokensArray} );");
        wr.WriteLine($"return {CompoundTokenStage.FunctionName}({VarNameValueTokensBuffer});");
        wr.WriteLine();

        wr.Indent--;
        wr.WriteLine("}");// end function
    }
}
