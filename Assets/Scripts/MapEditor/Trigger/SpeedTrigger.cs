using MapEditor.Timestamp;
using UnityEngine;

namespace MapEditor.Trigger
{
    public class SpeedTrigger : MonoBehaviour
    {
        public ITime Time;
        public double Second;
        public double Speed;
        public double Position;

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