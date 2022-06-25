using System;
using DataStructures.BiDictionary;
using Game.Scripts.Serialization.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Serialization.Converter
{
    public class ValueConverter : JsonConverter<ValueData>
    {
        private const string FieldType = "Type";
        private const string FieldValue = "Value";

        private readonly BiDictionary<string, Type> _types = new();

        public ValueConverter()
        {
            _types.Add("color", typeof(Color));
            _types.Add("vec2", typeof(Vector2));
            _types.Add("vec3", typeof(Vector3));
            _types.Add("vec4", typeof(Vector4));
            _types.Add("float", typeof(float));
        }

        public override void WriteJson(JsonWriter writer, ValueData value, JsonSerializer serializer)
        {
            JObject jObject = new()
            {
                new JProperty(FieldType, GetNameByType(value.Value.GetType())),
                new JProperty(FieldValue, JObject.FromObject(value.Value, serializer)),
            };
            jObject.WriteTo(writer);
        }

        public override ValueData ReadJson(JsonReader reader, Type objectType, ValueData existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            string type = (jObject[FieldType] ?? "").Value<string>();
            object obj = serializer.Deserialize(jObject[FieldValue]?.CreateReader()!, GetTypeByName(type));

            return new ValueData(obj);
        }

        private string GetNameByType(Type type)
        {
            return _types.ValueMap.TryGetValue(type, out string val) ? val : type.FullName;
        }
        
        private Type GetTypeByName(string name)
        {
            return _types.KeyMap.TryGetValue(name, out Type val) ? val : Type.GetType(name);
        }
    }
}