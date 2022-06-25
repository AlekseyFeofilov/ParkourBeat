using DataStructures.BiDictionary;
using Game.Scripts.Map.VisualEffect.Property;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

namespace Game.Scripts.Map.VisualEffect.Object
{
    public class MonoObject : MonoBehaviour
    {
        public readonly BiDictionary<string, IVisualProperty> Properties = new();

        private void OnEnable()
        {
            foreach (var field in GetType().GetDeclaredFields())
            {
                if (!field.HasAttribute(typeof(VisualPropertyAttribute))) continue;
                string id = field.GetAttribute<VisualPropertyAttribute>().Id;
                object property = field.GetValue(this);

                if (property is not IVisualProperty updatable) continue;
                
                updatable.Parent = this;
                Properties.Add(id, updatable);
            }
        }

        public void Reset()
        {
            foreach (var property in Properties.ValueMap.Keys)
            {
                property.Apply(property.GetDefault());
            }
        }

        public IVisualProperty GetPropertyById(string id)
        {
            return Properties.KeyMap[id];
        }

        public string GetIdByProperty(IVisualProperty property)
        {
            return Properties.ValueMap[property];
        }
    }
}