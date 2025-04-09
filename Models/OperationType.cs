using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CalculatorApi.Models
{
    /// <summary>
    /// Enumeration Class Pattern - defines the set of supported mathematical operations.
    /// 
    /// Decorated with <see cref="JsonConverterAttribute"/> to support string-based JSON serialization/deserialization.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationType
    {
        add,
        subtract,
        multiply,
        divide
    }
}
