namespace MetaParser.Trees;

public enum EMetaNodeKind
{
    // Base
    Root,
    // 
    DropConsumer,
    TokenConsumer,
    Literal,
    Pattern,
    PatternSequence,
    PatternRange,
    TokenPattern,
}
