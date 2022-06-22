namespace VisualEffect.Property
{
    public abstract class AbstractFloatProperty : AbstractVisualProperty<float>
    {
        private float _default;
        
        public override float Default
        {
            get => _default;
            set => Apply(_default = value);
        }

        protected override float Calculate(float multiplier, float from, float to)
        {
            return from + (to - from) * multiplier;
        }
    }
}