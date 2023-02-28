using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;
using Microsoft.CodeAnalysis.CSharp;

namespace MetaParser.Builders.Parser.Functions
{
    internal class ParsingLogic : IMetaCodeBuilder
    {
        public void WriteTo(MetaParserContext context)
        {
            const string VarNameValueTokensArray = "tokensArray";
            const string VarNameValueTokensBuffer = "tokensBuffer";
            var wr = context.writer;

            var tyInputBuffer = SyntaxFactory.ParseTypeName($"{CodeCommon.ReadOnlyMemory}<{context.InputTypeName}>");
            var tyTokenList = SyntaxFactory.ParseTypeName($"{MetaParserContext.TokenClassName}[]");
            

            wr.Write("public ");
            wr.WriteLine($"{tyTokenList} Parse({tyInputBuffer} {MetaParserContext.VarNameBufferMajor})");
            wr.WriteLine("{");
            wr.Indent++;
            // constant-tokens
            wr.WriteLine($"var {VarNameValueTokensArray} = {ConstantTokenStage.FunctionName}({MetaParserContext.VarNameBufferMajor});");
            // compound-tokens
            wr.WriteLine($"var {VarNameValueTokensBuffer} = new {CodeCommon.ReadOnlyMemory}<{MetaParserContext.TokenValueClassName}>( {VarNameValueTokensArray} );");
            wr.WriteLine($"return {CompoundTokenStage.FunctionName}({VarNameValueTokensBuffer});");
            wr.WriteLine();

            wr.Indent--;
            wr.WriteLine("}");// end function
        }
    }
}
