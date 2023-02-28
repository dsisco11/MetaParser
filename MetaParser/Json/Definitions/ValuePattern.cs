using System.Text.Json.Serialization;
using System;
using MetaParser.Contexts;
using MetaParser.Exceptions;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Collections.Generic;
using MetaParser.Patternization;
using System.Text.Json;

namespace MetaParser.Json.Definitions;

[JsonConverter(typeof(ValuePatternConverter))]
//[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
//[JsonDerivedType(typeof(ValuePatternConst))]
//[JsonDerivedType(typeof(ValuePatternRange), "range")]
//[JsonDerivedType(typeof(ValuePatternAlias), "pattern")]
internal abstract record ValuePattern : PatternDefinition;

internal sealed record ValuePatternConst : ValuePattern
{
    public string value;

    [JsonConstructor]
    public ValuePatternConst(string value)
    {
        this.value = value;
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Pattern.Empty;
        }

        if (value.Length == 1)
        {
            return new PatternConst(SymbolDisplay.FormatLiteral(value.ToCharArray()[0], true));
        }

        var consts = value.ToCharArray().Select(ch => new PatternConst(SymbolDisplay.FormatLiteral(ch, true))).ToArray();
        return new PatternGroup(EPatternCondition.AllOf, consts);
    }
}

internal sealed record ValuePatternRange : ValuePattern
{
    public readonly char begin;
    public readonly char end;

    [JsonConstructor]
    public ValuePatternRange(char start, char end)
    {
        this.begin = start;
        this.end = end;
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        var start = SymbolDisplay.FormatLiteral(begin, true);
        var stop = SymbolDisplay.FormatLiteral(end, true);
        return new PatternRange(start, stop);
    }
}

internal sealed record ValuePatternAlias : ValuePattern
{
    public readonly string name;

    [JsonConstructor]
    public ValuePatternAlias(string name)
    {
        this.name = name;
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        // Lookup alias in contexts patterns list
        // Write switch case pattern
        if (!context.Patterns.TryGetValue(name, out var patternList))
        {
            throw new PatternNotFoundException($"The specified pattern ('{name}') is not in the pattern definitions list!");
        }

        if (patternList.Length == 1)
        {
            return patternList.Single().Resolve(context);
        }
        else
        {
            List<Pattern> resolved = new(patternList.Length);
            for (int i = 0; i < patternList.Length; i++)
            {
                var definition = patternList[i];
                if (definition is not ValuePattern pattern)
                {
                    throw new Exception($"The pattern ('{name}') references the wrong pattern type, a value-type pattern is required");
                }

                resolved.Add(pattern.Resolve(context));
            }

            return new PatternGroup(EPatternCondition.OneOf, resolved.ToArray());
        }
    }
}

internal class ValuePatternConverter : JsonConverter<ValuePattern>
{
    public override ValuePattern? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.StartObject:
                {
                    if (!reader.Read())
                        throw new JsonException();

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException("Expected property name for json object");
                    }

                    var propertyName = reader.GetString();
                    switch (propertyName)
                    {
                        case "range":
                            {
                                return consume_range_pattern(ref reader);
                            }
                        case "pattern":
                            {
                                return consume_alias_pattern(ref reader);
                            }
                        default:
                            {
                                throw new JsonException("Expected 'range' property for item in compound token");
                            }
                    }
                }
            case JsonTokenType.String:
                {
                    var val = reader.GetString();
                    return val is not null ? new ValuePatternConst(val) : null;
                }
            default:
                {
                    throw new JsonException();
                }
        }

        throw new JsonException();
    }

    private static ValuePatternRange consume_range_pattern(ref Utf8JsonReader reader)
    {
        reader.Read();
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected array of strings for 'range' property");
        }

        reader.Read();
        var item1 = reader.GetString() ?? string.Empty;
        reader.Read();
        var item2 = reader.GetString() ?? string.Empty;

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Too many items in 'range' array, this property only takes two items: [start, end]");
        }

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Invalid property in 'range' item");
        }

        return new ValuePatternRange(item1[0], item2[0]);

    }

    private static ValuePatternAlias consume_alias_pattern(ref Utf8JsonReader reader)
    {
        reader.Read();
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string for 'pattern' property");
        }

        var item1 = reader.GetString() ?? string.Empty;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }
        }

        return new ValuePatternAlias(item1);

    }

    public override void Write(Utf8JsonWriter writer, ValuePattern value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
