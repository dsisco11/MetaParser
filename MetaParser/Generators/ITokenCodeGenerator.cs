using MetaParser.Contexts;

using System.CodeDom.Compiler;

namespace MetaParser.Generators
{
    internal interface ITokenCodeGenerator
    {
        void Generate(IndentedTextWriter wr, MetaParserTokenContext context);
    }
}