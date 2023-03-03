using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json;
using System;

namespace MetaParser.Json.Definitions;

internal sealed class ValuePatternDeclarationConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ValuePatternDeclaration);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new ValuePatternDeclarationConverter(options);
    }


    internal sealed class ValuePatternDeclarationConverter : JsonConverter<ValuePatternDeclaration>
    {
        private static readonly JsonConverter<string[]> s_stringArrayConverter = (JsonConverter<string[]>)JsonSerializerOptions.Default.GetConverter(typeof(string[]));
        private readonly Type _stringArrayType;
        //private readonly JsonConverter<IEnumerable<string>> _stringArrayConverter;

        public ValuePatternDeclarationConverter(JsonSerializerOptions options)
        {
            _stringArrayType = typeof(string[]);
            //_stringArrayConverter = (JsonConverter<IEnumerable<string>>)options.GetConverter(_stringArrayType);
        }

        public override ValuePatternDeclaration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    {
                        return null;
                    }
                case JsonTokenType.String:
                    {
                        return new ValuePatternDeclaration() { value = reader.GetString() };
                    }
                case JsonTokenType.StartArray:
                    {
                        return new ValuePatternDeclaration() { oneof = read_array(ref reader, typeToConvert, options) };
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

        ValuePatternDeclaration read_object(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                "value" => new ValuePatternDeclaration() { value = reader.GetString() },
                "range" => new ValuePatternDeclaration() { range = s_stringArrayConverter.Read(ref reader, _stringArrayType, options) },
                "oneof" => new ValuePatternDeclaration() { oneof = read_array(ref reader, typeToConvert, options) },
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

        ValuePatternDeclaration[] read_array(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<ValuePatternDeclaration> results = new();
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

            return Array.Empty<ValuePatternDeclaration>();
        }

        public override void Write(Utf8JsonWriter writer, ValuePatternDeclaration value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
