using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Serialization.Converter
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            JObject jObject = new()
            {
                new JProperty("x", value.x),
                new JProperty("y", value.y),
                new JProperty("z", value.z)
            };
            jObject.WriteTo(writer);
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            float x = (jObject["x"] ?? 0f).Value<float>();
            float y = (jObject["y"] ?? 0f).Value<float>();
            float z = (jObject["z"] ?? 0f).Value<float>();

            return new Vector3(x, y, z);
        }
    }
}