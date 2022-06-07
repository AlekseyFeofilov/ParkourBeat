using UnityEngine;

namespace MapEditor.Trigger
{
    public class BpmTrigger : MonoBehaviour
    {
        public double Second;
        public double Bpm;
        public double Beat;

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