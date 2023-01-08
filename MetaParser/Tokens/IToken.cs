namespace MetaParser.Tokens
{
    public interface IToken<T> : IEquatable<IToken<T>>
    {
        T[] Value { get; }
    }

    /// <summary>
    /// Any token which acts as a static instance, the contents of such a token are constant value declerations and are not assigned by the parser.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStaticToken<T> : IToken<T>
    {
        IStaticToken<T> static_instance { get; }
    }
}