using System.Text.Json;
using System;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Linq;
using MetaParser.Json.Attributes;

namespace MetaParser.Json.Converters;

internal class JsonPrimaryPropertyConverter : JsonConverterFactory
{
    private static PropertyInfo? GetMarkedProperty(Type typeToConvert)
    {
        return typeToConvert.GetProperties(BindingFlags.Public | BindingFlags.SetProperty)
                                           .Where(c => c.GetCustomAttribute<JsonPrimaryPropertyAttribute>() is not null)
                                           .SingleOrDefault(null);
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return GetMarkedProperty(typeToConvert) is not null;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var propertyType = GetMarkedProperty(typeToConvert) ?? throw new Exception($"Unable to find marked property for deserializing type: {typeToConvert}");
        return (JsonConverter)Activator.CreateInstance(
            typeof(ConverterInner<,>).MakeGenericType(typeToConvert, propertyType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null)!;
    }

    private sealed class ConverterInner<T, PropertyType> : JsonConverter<T>
    {
        private readonly JsonConverter<T> _valueConverter;
        private readonly JsonConverter<PropertyType> _propertyValueConverter;
        private readonly Type _type;
        private readonly PropertyInfo _property;

        public ConverterInner(JsonSerializerOptions options)
        {
            // For performance, use the existing converter.
            _valueConverter = (JsonConverter<T>)options
                .GetConverter(typeof(T));

            _type = typeof(T);
            _property = GetMarkedProperty(_type) ?? throw new Exception($"Unable to find marked property for deserializing type: {_type}");
            _propertyValueConverter = (JsonConverter<PropertyType>)options.GetConverter(_property.PropertyType);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    {
                        return default;
                    }
                case JsonTokenType.StartObject:
                    {
                        return _valueConverter.Read(ref reader, _type, options);
                    }
                default:
                    {
                        PropertyType? propertyValue = _propertyValueConverter.Read(ref reader, _property.PropertyType, options);

                        T instance = (T)Activator.CreateInstance(_type)!;
                        _property.SetValue(instance, propertyValue);
                        return instance;
                    }
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            _valueConverter.Write(writer, value, options);
        }
    }

}
