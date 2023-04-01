namespace MetaParser.Parsing.Constructs;

public enum EPatternKind
{
    /// <summary></summary>
    Value,
    /// <summary>'foo'</summary>
    Literal,
    /// <summary>TokenID</summary>
    Token,
    /// <summary>1 <= x <= 10</summary>
    Range,
    /// <summary>[ 'a', 'b', 'c' ]</summary>
    AllOf,
    /// <summary>['x' or 'y']</summary>
    OneOf,
};
