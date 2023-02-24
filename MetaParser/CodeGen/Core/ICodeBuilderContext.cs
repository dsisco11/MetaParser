using System.CodeDom.Compiler;

namespace MetaParser.CodeGen.Core;

internal interface ICodeBuilderContext
{
    IndentedTextWriter writer { get; }
}
