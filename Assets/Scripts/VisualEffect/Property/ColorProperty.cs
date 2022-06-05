using UnityEngine;

namespace VisualEffect.Property
{
    public abstract class AbstractColorProperty : AbstractVisualProperty<Color>
    {
        public override Color Default { get; set; } = Color.white;

        protected override void Update(float multiplier, Color from, Color to)
        {
            float r = from.r + (to.r - from.r) * multiplier;
            float g = from.g + (to.g - from.g) * multiplier;
            float b = from.b + (to.b - from.b) * multiplier;
            float a = from.a + (to.a - from.a) * multiplier;
            Apply(new Color(r, g, b, a));
        }
    }
}