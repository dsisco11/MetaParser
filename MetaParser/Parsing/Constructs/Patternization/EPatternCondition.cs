namespace MetaParser.Parsing.Constructs.Patternization;

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
