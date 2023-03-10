namespace MetaParser.Parsing.Constructs;

public enum EPatternCondition
{
    /// <summary>
    /// [ 'a', 'b', 'c' ]
    /// </summary>
    AllOf,
    /// <summary>
    /// ['x' or 'y']
    /// </summary>
    OneOf,
    /// <summary>
    /// 'foo'
    /// </summary>
    Only
};
