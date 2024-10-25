using MetaParser.Builders.Interfaces;

using System.CodeDom.Compiler;

namespace MetaParser.Core;

internal record struct CodeGenContext : ICodeBuilderContext
{
    #region Fields
    public CodeGenState State;
    #endregion

    #region Properties
    public ParserConfiguration Config { get; set; }
    public IndentedTextWriter? Writer { get; set; }
    #endregion

    #region Methods
    public void Increment_Active_Bufffer() => State.ActiveBuffer += 1;
    public void Decrement_Active_Bufffer() => State.ActiveBuffer -= 1;
    #endregion
}
