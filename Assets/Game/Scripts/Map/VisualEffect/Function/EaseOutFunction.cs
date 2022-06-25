namespace Game.Scripts.Map.VisualEffect.Function
{
    public class EaseOutFunction : ITimingFunction
    {
        public static readonly ITimingFunction EaseOut = new CubicBezierFunction(0, 0, .58, 1);
          
        public float Get(float time)
        {
            return EaseOut.Get(time);
        }
    }
}