using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json;
using System;

namespace MetaParser.Json.Definitions;

internal sealed class TokenPatternDeclarationConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(TokenPatternDeclaration);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new TokenPatternDeclarationConverter(options);
    }


    internal sealed class TokenPatternDeclarationConverter : JsonConverter<TokenPatternDeclaration>
    {
        public TokenPatternDeclarationConverter(JsonSerializerOptions options)
        {
        }

        public override TokenPatternDeclaration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    {
                        return null;
                    }
                case JsonTokenType.String:
                    {
                        return new TokenPatternDeclaration() { id = reader.GetString() };
                    }
                case JsonTokenType.StartArray:
                    {
                        return new TokenPatternDeclaration() { oneof = read_array(ref reader, typeToConvert, options) };
                    }
                case JsonTokenType.StartObject:
                    {
                        return read_object(ref reader, typeToConvert, options);
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }
        }

        TokenPatternDeclaration read_object(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!reader.Read())
            {
                throw new JsonException();
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var propertyName = reader.GetString();
            reader.Read();

            var result = propertyName switch
            {
                "id" => new TokenPatternDeclaration() { id = reader.GetString() },
                "oneof" => new TokenPatternDeclaration() { oneof = read_array(ref reader, typeToConvert, options) },
                _ => throw new NotSupportedException()
            };

            if (!reader.Read())
            {
                throw new JsonException();
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                // exhaust the rest of the object
                if (!reader.TrySkip())
                {
                    throw new JsonException();
                }
            }

            return result;
        }

        TokenPatternDeclaration[] read_array(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<TokenPatternDeclaration> results = new();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndArray:
                        {
                            return results.ToArray();
                        }
                    default:
                        {
                            var value = Read(ref reader, typeToConvert, options);
                            if (value is not null)
                            {
                                results.Add(value);
                            }
                            break;
                        }
                }
            }

            return Array.Empty<TokenPatternDeclaration>();
        }

        public override void Write(Utf8JsonWriter writer, TokenPatternDeclaration value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
