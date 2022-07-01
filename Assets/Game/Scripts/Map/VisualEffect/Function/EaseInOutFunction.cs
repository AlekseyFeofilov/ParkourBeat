namespace Game.Scripts.Map.VisualEffect.Function
{
    public class EaseInOutFunction : ITimingFunction
    {
        public static readonly ITimingFunction EaseInOut = new CubicBezierFunction(.42, 0, .58, 1);

        public float Get(float time)
        {
            return EaseInOut.Get(time);
        }
    }
}