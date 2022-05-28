using UnityEngine;

namespace VisualEffect.Property
{
    public abstract class AbstractColorProperty : AbstractVisualProperty<Color>
    {
        protected override void OnUpdate(float multiplier)
        {
            float r = Initial.r + (Target.r - Initial.r) * multiplier;
            float g = Initial.g + (Target.g - Initial.g) * multiplier;
            float b = Initial.b + (Target.b - Initial.b) * multiplier;
            float a = Initial.a + (Target.a - Initial.a) * multiplier;

            Value = new Color(r, g, b, a);
        }
    }
}