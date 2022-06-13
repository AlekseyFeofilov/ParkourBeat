using DataStructures.BiDictionary;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
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