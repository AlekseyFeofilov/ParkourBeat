using UnityEngine;

namespace VisualEffect.Property
{
    public abstract class AbstractColorProperty : IVisualProperty<Color>
    {
        public abstract Color Value { get; set; }

        public void Update()
        {
            
        }
        
        public void Transition(Color value, float duration)
        {
            
        }
    }
}