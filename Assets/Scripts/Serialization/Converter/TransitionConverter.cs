using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serialization.Data;
using UnityEngine;

namespace Serialization.Converter
{
    public class TransitionConverter : JsonConverter<TransitionData>
    {
        public override void WriteJson(JsonWriter writer, TransitionData value, JsonSerializer serializer)
        {
            JObject jObject = new()
            {
                new JProperty("Type", value.Type),
                new JProperty("Data", JObject.FromObject(value.Data, serializer))
            };
            jObject.WriteTo(writer);
        }

        public override TransitionData ReadJson(JsonReader reader, Type objectType, TransitionData existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            string type = (jObject["Type"] ?? 0f).Value<string>();
            Color data = serializer.Deserialize<Color>(jObject["Data"]?.CreateReader()!);

            return new TransitionData(type, data);
        }
    }
}