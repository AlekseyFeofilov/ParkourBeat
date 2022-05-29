using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Serialization
{
    [Serializable]
    public class TransitionData
    {
        public string Type;
        public Vector3 Data;
    }

    public class TransitionJsonConverter : JsonConverter<TransitionData>
    {
        public override void WriteJson(JsonWriter writer, TransitionData? value, JsonSerializer serializer)
        {
            JObject jObject = new()
            {
                new JProperty("Type", value!.Type),
                JObject.FromObject(value.Data, serializer)
            };
            jObject.WriteTo(writer);
        }

        public override TransitionData? ReadJson(JsonReader reader, Type objectType, TransitionData? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return new TransitionData();
        }
    }
}