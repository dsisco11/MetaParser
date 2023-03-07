using System.CodeDom.Compiler;

namespace MetaParser.Builders.Interfaces;

internal interface ICodeBuilderContext
{
    IndentedTextWriter writer { get; }
}
