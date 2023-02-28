using System.Text.Json.Serialization;
using System;
using MetaParser.Contexts;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using MetaParser.Patternization;
using System.Text.Json;
using MetaParser.Json.Attributes;

namespace MetaParser.Json.Definitions;

//[JsonConverter(typeof(ValuePatternConverter))]
[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
[JsonDerivedType(typeof(ValuePatternConst))]
[JsonDerivedType(typeof(ValuePatternRange), "range")]
internal abstract record ValuePattern : PatternDeclaration;

internal sealed record ValuePatternConst : ValuePattern
{
    [JsonPrimaryProperty]
    public string value { get; set; }

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
    public ValuePatternRange(string[] range)
    {
        this.begin = range[0].ToCharArray()[0];
        this.end = range[0].ToCharArray()[1];
    }

    public override Pattern Resolve(MetaParserContext context)
    {
        var start = SymbolDisplay.FormatLiteral(begin, true);
        var stop = SymbolDisplay.FormatLiteral(end, true);
        return new PatternRange(start, stop);
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
                        default:
                            {
                                throw new NotImplementedException($"Unrecognized property name({propertyName}) when deserializing '{nameof(ValuePattern)}' type");
                            }
                    }
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

        return new ValuePatternRange(new[] { item1, item2 });

    }

    public override void Write(Utf8JsonWriter writer, ValuePattern value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
