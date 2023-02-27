using MetaParser.CodeGen;
using MetaParser.CodeGen.Core;
using MetaParser.Contexts;

using Microsoft.CodeAnalysis;

namespace MetaParser.Builders.Parser.Functions
{
    internal class ConsumeNextToken : IMetaCodeBuilder
    {
        public const string Name = "TryConsume";

        public void WriteTo(MetaParserContext context)
        {
            var wr = context.writer;

            wr.WriteLine($"private static bool {Name}({CodeCommon.FormatReadOnlySpanBuffer(context.InputType)} {MetaParserContext.VarNameBufferMajor}, out {context.IdTypeName} id, out {CodeCommon.Format(SpecialType.System_Int32)} length)");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine($"if ({MetaParserContext.ConstantTokenProcessorFunctionName}({MetaParserContext.VarNameBufferMajor}, out var constId, out var constLen))");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("id = constId;");
            wr.WriteLine("length = constLen;");
            wr.WriteLine("return true;");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine($"else if ({MetaParserContext.CompoundTokenProcessorFunctionName}({MetaParserContext.VarNameBufferMajor}, out var compId, out var compLen))");
            wr.WriteLine("{");
            wr.Indent++;
            wr.WriteLine("id = compId;");
            wr.WriteLine("length = compLen;");
            wr.WriteLine("return true;");
            wr.Indent--;
            wr.WriteLine("}");
            wr.WriteLine();
            wr.WriteLine("id = default;");
            wr.WriteLine("length = default;");
            wr.WriteLine("return false;");

            wr.Indent--;
            wr.WriteLine("}");// end function
        }
    }
}
