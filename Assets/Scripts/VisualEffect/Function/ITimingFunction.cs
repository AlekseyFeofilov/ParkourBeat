namespace VisualEffect.Function
{
    public interface ITimingFunction
    {
        public static readonly LinearFunction Linear = new();
        public static readonly ITimingFunction Ease = new CubicBezierFunction(.25, .1, .25, 1);
        public static readonly ITimingFunction EaseIn = new CubicBezierFunction(.42, 0, 1, 1);
        public static readonly ITimingFunction EaseOut = new CubicBezierFunction(0, 0, .58, 1);
        public static readonly ITimingFunction EaseInOut = new CubicBezierFunction(.42, 0, .58, 1);
        
        float Get(float time);
    }
}