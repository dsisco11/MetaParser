namespace MetaParser.Builders.Interfaces;

internal interface ICodeBuilder<T> where T : ICodeBuilderContext
{
    public void WriteTo(T context);
    public ICodeBuilder<T> Then(ICodeBuilder<T> codeBuilder);
}
