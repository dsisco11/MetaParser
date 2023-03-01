using MetaParser.Contexts;
using MetaParser.Json.Attributes;
using MetaParser.Patternization;

using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

[JsonPolymorphic]
[JsonDerivedType(typeof(ValuePatternDeclaration))]
[JsonDerivedType(typeof(TokenPatternDeclaration))]
internal record PatternDeclaration
{
    #region Fields
    private EPatternCondition? _condition;
    private PatternDeclaration[] _items = Array.Empty<PatternDeclaration>();
    #endregion

    #region Properties
    [JsonPrimaryProperty]
    [JsonPropertyName("and")]
    public PatternDeclaration[]? and_items 
    {
        get 
        {
            return _condition switch
            {
                EPatternCondition.AllOf => _items,
                _ => null
            }; 
        }
        set 
        { 
            if (value is null) 
                return; 
            _items = value; 
            _condition = EPatternCondition.AllOf; 
        } 
    }

    [JsonPropertyName("or")]
    public PatternDeclaration[]? or_items
    {
        get
        {
            return _condition switch
            {
                EPatternCondition.OneOf => _items,
                _ => null
            };
        }
        set
        {
            if (value is null)
                return;
            _items = value;
            _condition = EPatternCondition.OneOf;
        }
    }
    #endregion

    public PatternDeclaration(PatternDeclaration[]? and_items = null, PatternDeclaration[]? or_items = null)
    {
        this.and_items = and_items;
        this.or_items = or_items;
    }

    public virtual Pattern Resolve(MetaParserContext context)
    {
        if (_items is not null && _items.Length > 0)
        {
            var resolvedItems = _items.Select(o => o.Resolve(context)).ToArray();
            return new PatternGroup(_condition.Value, resolvedItems);
        }

        return Pattern.Empty;
    }
}
