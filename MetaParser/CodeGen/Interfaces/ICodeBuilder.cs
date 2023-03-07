namespace MetaParser.CodeGen.Interfaces;

internal interface ICodeBuilder<T> where T : ICodeBuilderContext
{
    public void WriteTo(T context);
}
