namespace VisualEffect.Property
{
    public abstract class AbstractFloatProperty : AbstractVisualProperty<float>
    {
        protected override void OnUpdate(float multiplier)
        {
            Value = Initial + (Target - Initial) * multiplier;
        }
    }
}