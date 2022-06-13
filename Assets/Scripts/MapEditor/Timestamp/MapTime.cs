namespace MapEditor.Timestamp
{
    public class MapTime
    {
        public TimeUnit Unit;
        public double Value;

        public MapTime()
        {
        }
        
        public MapTime(TimeUnit unit, double value)
        {
            Unit = unit;
            Value = value;
        }

        public double ToSecond(ITimeConverter converter)
        {
            return converter.ToSecond(this);
        }

        public static MapTime Zero => new MapTime(TimeUnit.Second, 0);

        public static MapTime OfBeat(double beat)
        {
            return new MapTime(TimeUnit.Beat, beat);
        }
        
        public static MapTime OfSecond(double second)
        {
            return new MapTime(TimeUnit.Second, second);
        }
    }

    public enum TimeUnit
    {
        // Секунды - абсолютная величина
        Second,
        
        // Биты - относительная величина, зависит от карты
        Beat
    }
}