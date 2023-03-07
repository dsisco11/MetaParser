using System.CodeDom.Compiler;

namespace MetaParser.CodeGen.Interfaces;

internal interface ICodeBuilderContext
{
    IndentedTextWriter writer { get; }
}
