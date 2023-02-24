namespace MetaParser.CodeGen.Core;

internal interface ICodeBuilder<T> where T : ICodeBuilderContext
{
    public void WriteTo(T context);
}
