using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MetaParser.Json.Definitions;

//internal record ConsumerDeclaration<T> : IConsumerDefinition
//    where T : PatternDefinition
//{
//    #region Properties
//    [JsonPropertyName("$type")]
//    public string Type { get; set; }

//    [JsonPropertyName("start")]
//    public IEnumerable<T> Start { get; set; }

//    [JsonPropertyName("consume")]
//    public IEnumerable<T> Consume { get; set; }

//    [JsonPropertyName("stop")]
//    public IEnumerable<T> Stop { get; set; }

//    [JsonPropertyName("escape")]
//    public IEnumerable<T> Escape { get; set; }

//    IEnumerable<PatternDefinition> IConsumerDefinition.Start => Start as IEnumerable<PatternDefinition>;
//    IEnumerable<PatternDefinition> IConsumerDefinition.Consume => Consume as IEnumerable<PatternDefinition>;
//    IEnumerable<PatternDefinition> IConsumerDefinition.Stop => Stop as IEnumerable<PatternDefinition>;
//    IEnumerable<PatternDefinition> IConsumerDefinition.Escape => Escape as IEnumerable<PatternDefinition>;
//    #endregion

//    [JsonConstructor]
//    protected ConsumerDeclaration(string type, IEnumerable<T>? start, IEnumerable<T>? consume, IEnumerable<T>? stop, IEnumerable<T>? escape)
//    {
//        Type = type;
//        Start = start ?? Array.Empty<T>();
//        Consume = consume ?? Array.Empty<T>();
//        Stop = stop ?? Array.Empty<T>();
//        Escape = escape ?? Array.Empty<T>();
//    }
//}
