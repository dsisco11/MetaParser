using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using System.Reflection;

namespace MetaParser.Schemas.Structs
{
    internal class JsonOneOrManyConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return !typeToConvert.IsGenericType;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var elementType = typeToConvert.GetElementType();
            return (JsonConverter)Activator.CreateInstance(
                typeof(JsonOneOrManyConverterInner<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null)!;
        }

        private class JsonOneOrManyConverterInner<T> : JsonConverter<T[]>
        {
            private readonly JsonConverter<T> _valueConverter;
            private readonly Type _type;
            public JsonOneOrManyConverterInner(JsonSerializerOptions options)
            {
                // For performance, use the existing converter.
                _valueConverter = (JsonConverter<T>)options
                    .GetConverter(typeof(T));

                _type = typeof(T);
            }

            public override T[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.Null:
                        {
                            return null;
                        }
                    case JsonTokenType.StartArray:
                        {
                            LinkedList<T> retList = new();
                            while (reader.Read())
                            {
                                switch (reader.TokenType)
                                {
                                    case JsonTokenType.EndArray:
                                        {
                                            return retList.ToArray();
                                        }
                                    default:
                                        {
                                            var val = _valueConverter.Read(ref reader, _type, options);
                                            if (val is not null)
                                            {
                                                retList.AddLast(val);
                                            }
                                            break;
                                        }
                                }
                            }

                            return retList.ToArray();
                        }
                    default:
                        {
                            var val = _valueConverter.Read(ref reader, _type, options);
                            return val is not null ? (new T[] { val }) : Array.Empty<T>();
                        }
                }
            }

            public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
            {
                if (value.Length == 1)
                {
                    _valueConverter.Write(writer, value[0], options);
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
}
