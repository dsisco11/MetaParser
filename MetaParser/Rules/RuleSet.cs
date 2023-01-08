namespace MetaParser.Rules
{
    public sealed record RuleSet<T>(params ITokenRule<T>[] Items) where T : unmanaged, IEquatable<T>;
}
