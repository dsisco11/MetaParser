using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MetaParser.Schemas.Structs;

namespace MetaParser.JsonTypeConverters
{
    internal class CompoundItemConverter : JsonConverter<CompoundItemValue>
    {
        public override CompoundItemValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                        if (propertyName != "range")
                        {
                            throw new JsonException("Expected 'range' property for item in compound token");
                        }

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

                        return new CompoundValueRange(new[] { item1, item2 });
                    }
                case JsonTokenType.String:
                    {
                        var val = reader.GetString();
                        return val is not null ? new CompoundValueString(val) : (CompoundItemValue?)null;
                    }
                case JsonTokenType.StartArray:
                    {
                        LinkedList<string> retList = new();
                        while (reader.Read())
                        {
                            switch (reader.TokenType)
                            {
                                case JsonTokenType.EndArray:
                                    {
                                        return new CompoundValueRange(retList.ToArray());
                                    }
                                case JsonTokenType.String:
                                    {
                                        var val = reader.GetString();
                                        if (val is not null)
                                            retList.AddLast(val);
                                        break;
                                    }
                                default:
                                    {
                                        throw new JsonException();
                                    }
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new JsonException();
                    }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, CompoundItemValue value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
