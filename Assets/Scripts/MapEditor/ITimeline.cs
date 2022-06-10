using MapEditor.Timestamp;
using VisualEffect.Function;
using VisualEffect.Property;

namespace MapEditor
{
    public interface ITimeline : ITimeConverter
    {
        public SpeedTimestamp AddSpeedPoint(ITime time, double speed);

        public void RemoveSpeedPoint(ITime time);

        public double GetPositionBySecond(double second);

        public double GetSecondByPosition(double position);

        public BpmTimestamp AddBpmPoint(BaseTime time, double bpm);

        public void RemoveBpmPoint(BaseTime time);

        public double GetBeatBySecond(double second);

        public double GetSecondByBeat(double beat);

        public EffectTimestamp AddEffectPoint(
            ITime time,
            ITime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state);

        public void RemoveEffectPoint(EffectTimestamp effect);

        public void Move(double second);

        public void ResetMove();
    }
}