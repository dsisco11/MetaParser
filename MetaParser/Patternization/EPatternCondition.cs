namespace MetaParser.Patternization;

public enum EPatternCondition
{
    /// <summary>
    /// [ 'a', 'b', 'c' ]
    /// </summary>
    All,
    /// <summary>
    /// ['x' or 'y']
    /// </summary>
    Any,
    /// <summary>
    /// 'foo'
    /// </summary>
    Single
};
