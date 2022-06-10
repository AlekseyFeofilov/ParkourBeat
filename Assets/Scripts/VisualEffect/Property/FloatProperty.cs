namespace VisualEffect.Property
{
    public abstract class AbstractFloatProperty : AbstractVisualProperty<float>
    {
        public override float Default { get; set; }

        protected override float Calculate(float multiplier, float from, float to)
        {
            return from + (to - from) * multiplier;
        }
    }
}