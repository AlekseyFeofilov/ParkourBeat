using UnityEngine;

namespace VisualEffect.Property
{
    public abstract class AbstractVector2Property : AbstractVisualProperty<Vector2>
    {
        protected override void OnUpdate(float multiplier)
        {
            float x = Initial.x + (Target.x - Initial.x) * multiplier;
            float y = Initial.y + (Target.y - Initial.y) * multiplier;

            Value = new Vector2(x, y);
        }
    }
    
    public abstract class AbstractVector3Property : AbstractVisualProperty<Vector3>
    {
        protected override void OnUpdate(float multiplier)
        {
            float x = Initial.x + (Target.x - Initial.x) * multiplier;
            float y = Initial.y + (Target.y - Initial.y) * multiplier;
            float z = Initial.z + (Target.z - Initial.z) * multiplier;

            Value = new Vector3(x, y, z);
        }
    }
    
    public abstract class AbstractVector4Property : AbstractVisualProperty<Vector4>
    {
        protected override void OnUpdate(float multiplier)
        {
            float x = Initial.x + (Target.x - Initial.x) * multiplier;
            float y = Initial.y + (Target.y - Initial.y) * multiplier;
            float z = Initial.z + (Target.z - Initial.z) * multiplier;
            float w = Initial.w + (Target.w - Initial.w) * multiplier;

            Value = new Vector4(x, y, z, w);
        }
    }
}