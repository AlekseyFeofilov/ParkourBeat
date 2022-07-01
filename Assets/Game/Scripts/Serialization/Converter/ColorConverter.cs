using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Serialization.Converter
{
    public class ColorConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            JObject jObject = new()
            {
                new JProperty("r", value.r),
                new JProperty("g", value.g),
                new JProperty("b", value.b),
                new JProperty("a", value.a)
            };
            jObject.WriteTo(writer);
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            float r = (jObject["r"] ?? 0f).Value<float>();
            float g = (jObject["g"] ?? 0f).Value<float>();
            float b = (jObject["b"] ?? 0f).Value<float>();
            float a = (jObject["a"] ?? 0f).Value<float>();

            return new Color(r, g, b, a);
        }
    }
}