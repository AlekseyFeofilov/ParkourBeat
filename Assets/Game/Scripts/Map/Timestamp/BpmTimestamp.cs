namespace Game.Scripts.Map.Timestamp
{
    public class BpmTimestamp
    {
        public readonly double Second;
        public readonly double Bpm;
        public readonly double Beat;

        public BpmTimestamp(double second, double bpm, double beat)
        {
            Second = second;
            Bpm = bpm;
            Beat = beat;
        }

        public double GetSecond(double beat)
        {
            double x = beat - Beat;
            double y = x / Bpm * 60;
            return y + Second;
        }

        public double GetBeat(double second)
        {
            double x = second - Second;
            double y = x * Bpm / 60;
            return y + Beat;
        }
    }
}