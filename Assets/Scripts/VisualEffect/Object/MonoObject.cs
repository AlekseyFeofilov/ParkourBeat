using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class MonoObject : MonoBehaviour
    {
        private readonly ICollection<IVisualProperty> _properties = new List<IVisualProperty>();

        private void OnEnable()
        {
            foreach (var field in GetType().GetDeclaredFields())
            {
                if (!field.HasAttribute(typeof(VisualPropertyAttribute))) continue;
                object property = field.GetValue(this);
                
                if (property is IVisualProperty updatable)
                {
                    _properties.Add(updatable);
                }
            }
        }
    }
}