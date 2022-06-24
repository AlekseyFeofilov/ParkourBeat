namespace VisualEffect.Function
{
    public interface ITimingFunction
    {
        public static readonly LinearFunction Linear = new();
        public static readonly EaseFunction Ease = new();
        public static readonly EaseInFunction EaseIn = new();
        public static readonly EaseOutFunction EaseOut = new();
        public static readonly EaseInOutFunction EaseInOut = new();
        
        float Get(float time);
    }
}