namespace MetaParser.Syntax;

internal interface IAstTreeFilter
{
    public EFilterResult Filter(RedNode node);
}
