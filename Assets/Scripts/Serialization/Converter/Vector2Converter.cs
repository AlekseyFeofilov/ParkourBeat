using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Serialization.Converter
{
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            JObject jObject = new()
            {
                new JProperty("x", value.x),
                new JProperty("y", value.y),
            };
            jObject.WriteTo(writer);
        }

        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            float x = (jObject["x"] ?? 0f).Value<float>();
            float y = (jObject["y"] ?? 0f).Value<float>();

            return new Vector2(x, y);
        }
    }
}