using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json;
using System;

namespace MetaParser.Json.Definitions;

internal sealed class PatternDeclarationConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(PatternDeclaration);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new PatternDeclarationConverter(options);
    }


    internal sealed class PatternDeclarationConverter : JsonConverter<PatternDeclaration>
    {
        private static readonly JsonConverter<string[]> s_stringArrayConverter = (JsonConverter<string[]>)JsonSerializerOptions.Default.GetConverter(typeof(string[]));
        private readonly Type _stringArrayType;
        //private readonly JsonConverter<IEnumerable<string>> _stringArrayConverter;

        public PatternDeclarationConverter(JsonSerializerOptions options)
        {
            _stringArrayType = typeof(string[]);
            //_stringArrayConverter = (JsonConverter<IEnumerable<string>>)options.GetConverter(_stringArrayType);
        }

        public override PatternDeclaration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    {
                        return null;
                    }
                case JsonTokenType.String:
                    {
                        return new PatternDeclaration() { value = reader.GetString() };
                    }
                case JsonTokenType.StartArray:
                    {
                        return new PatternDeclaration() { oneof = read_array(ref reader, typeToConvert, options) };
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

        PatternDeclaration read_object(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                "value" => new PatternDeclaration() { value = reader.GetString() },
                "range" => new PatternDeclaration() { range = s_stringArrayConverter.Read(ref reader, _stringArrayType, options) },
                "oneof" => new PatternDeclaration() { oneof = read_array(ref reader, typeToConvert, options) },
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

        PatternDeclaration[] read_array(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<PatternDeclaration> results = new();
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

            return Array.Empty<PatternDeclaration>();
        }

        public override void Write(Utf8JsonWriter writer, PatternDeclaration value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
