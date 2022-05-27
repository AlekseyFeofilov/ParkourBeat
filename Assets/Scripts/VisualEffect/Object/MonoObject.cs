using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class MonoObject : MonoBehaviour
    {
        private readonly ICollection<IVisualUpdatable> _properties = new List<IVisualUpdatable>();

        private void OnEnable()
        {
            foreach (var field in GetType().GetDeclaredFields())
            {
                if (!field.HasAttribute(typeof(VisualPropertyAttribute))) continue;
                object property = field.GetValue(this);
                if (property is IVisualUpdatable updatable)
                {
                    _properties.Add(updatable);   
                }
            }
        }

        private void Update()
        {
            foreach (var updatable in _properties)
            {
                updatable.Update();
            }
        }
    }
}