using System;
using System.Text.RegularExpressions;
using DataStructures.BiDictionary;
using Newtonsoft.Json;
using Serialization.Data;

namespace Serialization.Converter
{
    public class TimeConverter : JsonConverter<TimeData>
    {
        private const string ValueRegex = @"^[\d,]+";   // language=regex
        private const string UnitRegex = @"\D+$";    // language=regex

        private readonly BiDictionary<string, TimeUnit> timeUnitRegistry = new();

        public TimeConverter()
        {
            timeUnitRegistry.Add("s", TimeUnit.Second);
            timeUnitRegistry.Add("b", TimeUnit.Beat);
        }

        public override void WriteJson(JsonWriter writer, TimeData value, JsonSerializer serializer)
        {
            string unit = timeUnitRegistry.ValueMap[value.Unit];
            writer.WriteValue(value.Value + unit);
        }

        public override TimeData ReadJson(JsonReader reader, Type objectType, TimeData existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            TimeData timeData = new();
            string read = (string) reader.Value;

            if (read == null) return timeData;

            timeData.Value = double.Parse(Regex.Match(read, ValueRegex).Value);
            timeData.Unit = timeUnitRegistry.KeyMap[Regex.Match(read, UnitRegex).Value];
            return timeData;
        }
    }
}