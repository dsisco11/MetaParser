using MetaParser.Parsing.Definitions;

namespace MetaParser.Parsing
{
    public sealed record TokenRuleSet<Ty>(params ITokenRule<Ty>[] Items) where Ty : unmanaged, IEquatable<Ty>;
}
