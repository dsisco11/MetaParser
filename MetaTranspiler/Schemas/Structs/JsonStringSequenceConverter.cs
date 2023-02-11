using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;

namespace MetaTranspiler.Schemas.Structs
{
    public class JsonStringSequenceConverter : JsonConverter<string[]>
    {
        public override string[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    {
                        return null;
                    }
                case JsonTokenType.StartArray:
                    {
                        LinkedList<string> retList = new();
                        while (reader.Read())
                        {
                            switch (reader.TokenType)
                            {
                                case JsonTokenType.String:
                                    {
                                        var str = reader.GetString();
                                        if (str is not null)
                                        {
                                            retList.AddLast(str);
                                        }
                                        break;
                                    }
                                case JsonTokenType.EndArray:
                                    {
                                        return retList.ToArray();
                                    }
                                default:
                                    {
                                        throw new JsonException();
                                    }
                            }
                        }
                        break;
                    }
                case JsonTokenType.String:
                    {
                        var str = reader.GetString();
                        return str is not null ? (new string[] { str }) : Array.Empty<string>();
                    }
                default:
                    {
                        throw new JsonException("Expected either string or start of string array");
                    }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
        {
            if (value.Length == 1)
            {
                writer.WriteStringValue(value[0]);
                return;
            }

            writer.WriteStartArray();
            foreach (var item in value)
            {
                writer.WriteStringValue(item);
            }
            writer.WriteEndArray();
        }
    }
}
