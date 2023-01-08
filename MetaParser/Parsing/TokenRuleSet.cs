using MetaParser.Rules;

namespace MetaParser.Parsing
{
    public sealed record TokenRuleSet<T>(params ITokenRule<T>[] Items) where T : unmanaged, IEquatable<T>;
}
