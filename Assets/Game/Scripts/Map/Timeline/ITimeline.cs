using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Function;
using Game.Scripts.Map.VisualEffect.Property;

namespace Game.Scripts.Map.Timeline
{
    public interface ITimeline : ITimeConverter, IStored
    {
        public SpeedTimestamp AddSpeedPoint(MapTime time, double speed);

        public void RemoveSpeedPoint(MapTime time);

        public double GetPositionBySecond(double second);

        public double GetSecondByPosition(double position);

        public BpmTimestamp AddBpmPoint(MapTime time, double bpm);

        public void RemoveBpmPoint(MapTime time);

        public double GetBeatBySecond(double second);

        public double GetSecondByBeat(double beat);

        public EffectTimestamp AddEffectPoint(
            MapTime beginTime,
            MapTime endTime,
            ITimingFunction function,
            IVisualProperty property,
            object state);

        public void RemoveEffectPoint(EffectTimestamp effect);

        public void Move(double second);

        public void ResetMove();
    }
}