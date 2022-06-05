namespace VisualEffect.Property
{
    public abstract class AbstractFloatProperty : AbstractVisualProperty<float>
    {
        public override float Default { get; set; }

        protected override void Update(float multiplier, float from, float to)
        {
            Apply(from + (to - from) * multiplier);
        }
    }
}