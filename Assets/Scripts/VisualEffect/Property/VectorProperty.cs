using UnityEngine;

namespace VisualEffect.Property
{
    public abstract class AbstractVector2Property : AbstractVisualProperty<Vector2>
    {
        public override Vector2 Default { get; set; } = new(0, 0);

        protected override Vector2 Calculate(float multiplier, Vector2 from, Vector2 to)
        {
            float x = from.x + (to.x - from.x) * multiplier;
            float y = from.y + (to.y - from.y) * multiplier;
            return new Vector2(x, y);
        }
    }
    
    public abstract class AbstractVector3Property : AbstractVisualProperty<Vector3>
    {
        public override Vector3 Default { get; set; } = new(0, 0, 0);

        protected override Vector3 Calculate(float multiplier, Vector3 from, Vector3 to)
        {
            float x = from.x + (to.x - from.x) * multiplier;
            float y = from.y + (to.y - from.y) * multiplier;
            float z = from.z + (to.z - from.z) * multiplier;
            return new Vector3(x, y, z);
        }
    }
    
    public abstract class AbstractVector4Property : AbstractVisualProperty<Vector4>
    {
        public override Vector4 Default { get; set; } = new(0, 0, 0, 0);

        protected override Vector4 Calculate(float multiplier, Vector4 from, Vector4 to)
        {
            float x = from.x + (to.x - from.x) * multiplier;
            float y = from.y + (to.y - from.y) * multiplier;
            float z = from.z + (to.z - from.z) * multiplier;
            float w = from.w + (to.w - from.w) * multiplier;
            return new Vector4(x, y, z, w);
        }
    }
}