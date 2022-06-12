namespace MapEditor.Timestamp
{
    public class SpeedTimestamp
    {
        public readonly MapTime Time;
        public readonly double Second;
        public readonly double Speed;
        public readonly double Position;

        public SpeedTimestamp(MapTime time, double second, double speed, double position)
        {
            Time = time;
            Second = second;
            Speed = speed;
            Position = position;
        }

        public double GetSecond(double position)
        {
            double x = position - Position;
            double y = x / Speed;
            return y + Second;
        }

        public double GetPosition(double second)
        {
            double x = second - Second;
            double y = x * Speed;
            return y + Position;
        }
    }
}