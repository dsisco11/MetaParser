using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Linq;

namespace MetaParser.Json.JsonTypeConverters;
/// <summary>
/// Allows using either a single value or OR an array for a property.
/// </summary>
internal class JsonEnumerableConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        return typeToConvert.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var genericArgs = typeToConvert.GetGenericArguments();
        var elementType = genericArgs[0];

        return (JsonConverter)Activator.CreateInstance(
            typeof(ConverterInner<>).MakeGenericType(elementType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null)!;
    }

    private class ConverterInner<T> : JsonConverter<IEnumerable<T>>
    {
        private readonly JsonConverter<T> _valueConverter;
        private readonly Type _type;
        public ConverterInner(JsonSerializerOptions options)
        {
            // For performance, use the existing converter.
            _valueConverter = (JsonConverter<T>)options
                .GetConverter(typeof(T));

            _type = typeof(T);
        }

        public override IEnumerable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    {
                        return null;
                    }
                case JsonTokenType.StartArray:
                    {
                        List<T> retList = new();
                        while (reader.Read())
                        {
                            switch (reader.TokenType)
                            {
                                case JsonTokenType.EndArray:
                                    {
                                        return retList;
                                    }
                                default:
                                    {
                                        var val = _valueConverter.Read(ref reader, _type, options);
                                        if (val is not null)
                                        {
                                            retList.Add(val);
                                        }
                                        break;
                                    }
                            }
                        }

                        return retList;
                    }
                default:
                    {
                        var val = _valueConverter.Read(ref reader, _type, options);
                        return val is not null ? (new T[] { val }) : Array.Empty<T>();
                    }
            }
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
        {
            if (value.Count() == 1)
            {
                _valueConverter.Write(writer, value.First(), options);
                return;
            }

            writer.WriteStartArray();
            foreach (var item in value)
            {
                _valueConverter.Write(writer, item, options);
            }
            writer.WriteEndArray();
        }
    }

}
