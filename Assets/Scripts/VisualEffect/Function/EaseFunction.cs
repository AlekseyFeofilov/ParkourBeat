namespace VisualEffect.Function
{
    public class EaseFunction : ITimingFunction
    {
        public static readonly ITimingFunction Ease = new CubicBezierFunction(.25, .1, .25, 1);
        
        public float Get(float time)
        {
            return Ease.Get(time);
        }
    }
}