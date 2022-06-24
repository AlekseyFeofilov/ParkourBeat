using System;
using System.Text.RegularExpressions;
using DataStructures.BiDictionary;
using MapEditor.Timestamp;
using Newtonsoft.Json;

namespace Serialization.Converter
{
    public class TimeConverter : JsonConverter<MapTime>
    {
        private const string ValueRegex = @"^[\d,]+";   // language=regex
        private const string UnitRegex = @"\D+$";    // language=regex

        private readonly BiDictionary<string, TimeUnit> timeUnitRegistry = new();

        public TimeConverter()
        {
            timeUnitRegistry.Add("s", TimeUnit.Second);
            timeUnitRegistry.Add("b", TimeUnit.Beat);
        }

        public override void WriteJson(JsonWriter writer, MapTime value, JsonSerializer serializer)
        {
            string unit = timeUnitRegistry.ValueMap[value.Unit];
            writer.WriteValue(value.Value + unit);
        }

        public override MapTime ReadJson(JsonReader reader, Type objectType, MapTime existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            MapTime timeData = new();
            string read = (string) reader.Value;

            if (read == null) return timeData;

            try
            {
                timeData.Value = double.Parse(Regex.Match(read, ValueRegex).Value);
            }
            catch (FormatException)
            {
                timeData.Value = 0;
            }
            timeData.Unit = timeUnitRegistry.KeyMap[Regex.Match(read, UnitRegex).Value];
            return timeData;
        }
    }
}