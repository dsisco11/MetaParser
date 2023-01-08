using MetaParser.Parsing.Definitions;

namespace MetaParser.Parsing
{
    public sealed record TokenRuleSet<T>(params ITokenRule<T>[] Items) where T : unmanaged, IEquatable<T>;
}
