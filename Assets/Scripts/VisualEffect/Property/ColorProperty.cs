using UnityEngine;

namespace VisualEffect.Property
{
    public abstract class AbstractColorProperty : AbstractVisualProperty<Color>
    {
        private Color _default = Color.gray;
        
        public override Color Default
        {
            get => _default;
            set => Apply(_default = value);
        }

        protected override Color Calculate(float multiplier, Color from, Color to)
        {
            float r = from.r + (to.r - from.r) * multiplier;
            float g = from.g + (to.g - from.g) * multiplier;
            float b = from.b + (to.b - from.b) * multiplier;
            float a = from.a + (to.a - from.a) * multiplier;
            return new Color(r, g, b, a);
        }
    }
}