namespace MetaParser.Builders.Interfaces;

internal interface ICodeBuilder<T> where T : ICodeBuilderContext
{
    public void WriteTo(T context);

    /// <summary> Adds the specified buidlers instructions BEFORE this builders instructions </summary>
    public ICodeBuilder<T> Before(ICodeBuilder<T> codeBuilder);
    /// <summary> Adds the specified buidlers instructions AFTER this builders instructions </summary>
    public ICodeBuilder<T> After(ICodeBuilder<T> codeBuilder);
    /// <summary> Adds the specified buidlers instructions WITHIN this builders instructions </summary>
    public ICodeBuilder<T> And(ICodeBuilder<T> codeBuilder);
}
