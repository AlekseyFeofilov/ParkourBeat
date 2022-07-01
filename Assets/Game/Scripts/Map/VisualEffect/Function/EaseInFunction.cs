namespace Game.Scripts.Map.VisualEffect.Function
{
    public class EaseInFunction : ITimingFunction
    {
        public static readonly ITimingFunction EaseIn = new CubicBezierFunction(.42, 0, 1, 1);
         
        public float Get(float time)
        {
            return EaseIn.Get(time);
        }
    }
}