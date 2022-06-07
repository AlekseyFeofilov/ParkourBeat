using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapEditor.Timestamp;
using MapEditor.Trigger;
using Serialization.Data;
using UnityEngine;
using Utils;
using VisualEffect.Function;
using VisualEffect.Property;

namespace MapEditor
{
    public class Timeline : MonoBehaviour, ITimeConverter, ITimeline
    {
        private const double PlaneScale = 10.0;
        
        private readonly List<BpmTrigger> _bpmPoints = new();
        private readonly List<SpeedTrigger> _speedPoints = new();
        private readonly List<EffectTrigger> _beginSortedEffectPoints = new();
        private readonly List<EffectTrigger> _endSortedEffectPoints = new();
        
        private readonly ISet<EffectTrigger> _activeEffects = new HashSet<EffectTrigger>();
        private int _indexByBeginSorted;
        private int _indexByEndSorted;
        private double _lastSecond;
        
        [SerializeField]
        private GameObject beatPrefab;
        
        [SerializeField]
        private GameObject speedPrefab;
        
        [SerializeField]
        private GameObject bpmPrefab;
        
        [SerializeField]
        private GameObject triggerPrefab;

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
        private void Start()
        {
            Transform transform = this.transform;
            Vector3 position = transform.position;
            Vector3 localScale = transform.localScale * (float) PlaneScale;

            double beginPos = position.x - localScale.x / 2;
            double endPos = position.x + localScale.x / 2;

            double beginTime = GetSecondByPosition(beginPos);
            double endTime = GetSecondByPosition(endPos);

            int beginBeat = (int) Math.Floor(GetBeatBySecond(beginTime));
            int endBeat = (int) Math.Ceiling(GetBeatBySecond(endTime));

            for (int beat = beginBeat; beat <= endBeat; beat++)
            {
                double time = GetSecondByBeat(beat);
                double pos = GetPositionBySecond(time);
                GameObject obj;

                obj = Instantiate(beatPrefab, 
                    new Vector3((float) pos, position.y + .001f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                    );
                obj.transform.parent = transform;

                obj = Instantiate(beatPrefab, 
                    new Vector3((float) pos, position.y + localScale.z / 2, position.z + localScale.z / 2),
                    Quaternion.identity
                    );
                obj.transform.parent = transform;
            }

            for (int i = 1; i < _speedPoints.Count; i++)
            {
                double time = _speedPoints[i].Time.ToSecond(this);
                double pos = _speedPoints[i - 1].GetPosition(time);
                GameObject obj;
                
                obj = Instantiate(speedPrefab, 
                    new Vector3((float) pos, position.y + .002f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                );
                obj.transform.parent = transform;
                
                obj = Instantiate(speedPrefab, 
                    new Vector3((float) pos, position.y + localScale.z / 2, position.z + localScale.z / 2),
                    Quaternion.identity
                );
                obj.transform.parent = transform;
            }
            
            for (int i = 1; i < _bpmPoints.Count; i++)
            {
                double second = _bpmPoints[i].Second;
                double pos = GetPositionBySecond(second);
                GameObject obj;
                
                obj = Instantiate(bpmPrefab, 
                    new Vector3((float) pos, position.y + .003f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                );
                obj.transform.parent = transform;
                
                obj = Instantiate(bpmPrefab, 
                    new Vector3((float) pos, position.y + localScale.z / 2, position.z + localScale.z / 2),
                    Quaternion.identity
                );
                obj.transform.parent = transform;
            }
        }
                
        public SpeedTrigger AddSpeedPoint(ITime time, double speed)
        {
            double second = time.ToSecond(this);
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Second, second);
            double position = index < 0 ? 0 : _speedPoints[index]
                .GetPosition(second);
            
            SpeedTrigger point = CreateSpeedTrigger(time, second, speed, position);
            _speedPoints.Insert(index + 1, point);
            RecalcSpeedPointsFromIndex(index + 2);
            return point;
        }

        public void RemoveSpeedPoint(ITime time)
        {
            double second = time.ToSecond(this);
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Second, second);
            if (index < 0) return;
            
            _speedPoints.RemoveAt(index);
            RecalcSpeedPointsFromIndex(index);
        }

        private void RecalcSpeedPointsFromIndex(int index)
        {
            for (int i = index; i < _speedPoints.Count; i++)
            {
                double currentSecond = _speedPoints[i].Second;
                _speedPoints[i].Position = _speedPoints[i - 1]
                    .GetPosition(currentSecond);
                // TODO move position
            }
        }

        public double GetPositionBySecond(double second)
        {
            SpeedTrigger point = SearchSpeedPointBySecond(second);
            if (point == null) return -1;
            return point.GetPosition(second);
        }

        private SpeedTrigger SearchSpeedPointBySecond(double second)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Time.ToSecond(this), second);
            if (index < 0) return null;
            return _speedPoints[index];
        }

        public double GetSecondByPosition(double position)
        {
            SpeedTrigger point = SearchSpeedPointByPosition(position);
            if (point == null) return -1;
            return point.GetSecond(position);
        }
        
        private SpeedTrigger SearchSpeedPointByPosition(double position)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Position, position);
            if (index < 0) return null;
            return _speedPoints[index];
        }
        
        public BpmTrigger AddBpmPoint(BaseTime time, double bpm)
        {
            double second;
            double beat;
            int index;
            
            switch (time.Unit)
            {
                case TimeUnit.Second:
                    second = time.Value;
                    index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints,
                        e => e.Second, second);
                    beat = index < 0 ? 0 : _bpmPoints[index].GetBeat(second);
                    break;
                case TimeUnit.Beat:
                    beat = time.Value;
                    index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints,
                        e => e.Beat, beat);
                    second = index < 0 ? 0 : _bpmPoints[index].GetSecond(beat);
                    break;
                default:
                    throw new InvalidOperationException("unsupported time unit: " + time.Unit);
            }
            
            BpmTrigger point = CreateBpmTrigger(second, bpm, beat);
            _bpmPoints.Insert(index + 1, point);
            RecalcBpmPointsFromIndex(index + 2);
            RecalcSpeedPointsAfterBpmChange();
            return point;
        }

        public void RemoveBpmPoint(BaseTime time)
        {
            int index;
            
            switch (time.Unit)
            {
                case TimeUnit.Second:
                    index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, 
                        e => e.Second, time.Value);
                    break;
                case TimeUnit.Beat:
                    index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, 
                        e => e.Beat, time.Value);
                    break;
                default:
                    throw new InvalidOperationException("unsupported time unit: " + time.Unit);
            }
            if (index < 0) return;
            
            _bpmPoints.RemoveAt(index);
            RecalcBpmPointsFromIndex(index);
            RecalcSpeedPointsAfterBpmChange();
        }

        private void RecalcBpmPointsFromIndex(int index)
        {
            for (int i = index; i < _bpmPoints.Count; i++)
            {
                double currentSecond = _bpmPoints[i].Second;
                _bpmPoints[i].Beat = _bpmPoints[i - 1]
                    .GetBeat(currentSecond);
                // TODO move position
            }
        }
        
        // TODO оптимизировать
        // Например пересчитывать только для поинтов скорости, время которых больше,
        // чем у поинта смены BPM
        private void RecalcSpeedPointsAfterBpmChange()
        {
            for (int i = 1; i < _speedPoints.Count; i++)
            {
                ITime currentTime = _speedPoints[i].Time;
                double currentSecond = currentTime.ToSecond(this);
                _speedPoints[i].Position = _speedPoints[i - 1]
                    .GetPosition(currentSecond);
                // TODO move position
            }
        }
        
        public double GetBeatBySecond(double second)
        {
            BpmTrigger point = SearchBpmPointBySecond(second);
            if (point == null) return -1;
            return point.GetBeat(second);
        }

        private BpmTrigger SearchBpmPointBySecond(double second)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, 
                e => e.Second, second);
            if (index < 0) return null;
            return _bpmPoints[index];
        }

        public double GetSecondByBeat(double beat)
        {
            BpmTrigger point = SearchBpmPointByBeat(beat);
            if (point == null) return -1;
            return point.GetSecond(beat);
        }

        private BpmTrigger SearchBpmPointByBeat(double beat)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, 
                e => e.Beat, beat);
            if (index < 0) return null;
            return _bpmPoints[index];
        }

        public EffectTrigger AddEffectPoint(
            ITime time,
            ITime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            EffectTrigger effect = CreateEffectTrigger(time, duration, function, property, state);
            
            // Добавляем эффект в общий список
            ITime begin = effect.Time;
            ITime end = effect.Time + effect.Duration;
            
            double beginSecond = begin.ToSecond(this);
            double endSecond = end.ToSecond(this);
            
            int beginIndex = CollectionUtils.SearchBinaryInRangeList(_beginSortedEffectPoints, 
                e => e.Time.ToSecond(this), beginSecond);
            int endIndex = CollectionUtils.SearchBinaryInRangeList(_endSortedEffectPoints, 
                e => e.Time.ToSecond(this), endSecond);
            
            _beginSortedEffectPoints.Insert(beginIndex + 1, effect);
            _endSortedEffectPoints.Insert(endIndex + 1, effect);
            
            // Добавляем эффект в список свойства
            // TODO если сложность O(n), то какой смысл в доп. списке?
            var points = effect.Property.Points;
            int index = CollectionUtils.SearchBinaryInRangeList(points, 
                e => e.Time.ToSecond(this), beginSecond);

            if (index < 0) effect.FromState = effect.Property.GetDefault();
            else effect.FromState = points[index].ToState;
            if (index + 1 < points.Count) points[index + 1].FromState = effect.ToState;
            
            points.Insert(index + 1, effect);
            return effect;
        }

        public void RemoveEffectPoint(EffectTrigger effect)
        {
            // Убираем эффект из общего списка
            _beginSortedEffectPoints.Remove(effect);
            _endSortedEffectPoints.Remove(effect);
            
            // Убираем эффект из списка свойства
            // TODO если сложность O(n), то какой смысл в доп. списке?
            var points = effect.Property.Points;
            int index = points.BinarySearch(effect);

            if (index > 0 && index + 1 < points.Count)
            {
                points[index + 1].FromState = points[index - 1].ToState;
            }
            else if (index == 0 && index + 1 < points.Count)
            {
                points[index + 1].FromState = effect.Property.GetDefault();
            }

            points.RemoveAt(index);
        }

        public void Move(double second)
        {
            if (second >= _lastSecond) MoveForth(second);
            else MoveBack(second);
            _lastSecond = second;
        }

        public void ResetMove()
        {
            Move(0);
        }

        private void MoveForth(double second)
        {
            var beginSorted = _beginSortedEffectPoints;
            var endSorted = _endSortedEffectPoints;

            while (_indexByBeginSorted < beginSorted.Count &&
                   beginSorted[_indexByBeginSorted].Time.ToSecond(this) <= second)
            {
                _activeEffects.Add(beginSorted[_indexByBeginSorted++]);
            }

            foreach (var effect in _activeEffects)
            {
                effect.UpdateEffect(second, this);
            }

            while (_indexByEndSorted < endSorted.Count &&
                   endSorted[_indexByEndSorted].EndTime.ToSecond(this) <= second)
            {
                _activeEffects.Remove(endSorted[_indexByEndSorted++]);
            }
        }
        
        private void MoveBack(double second)
        {
            var endSorted = _endSortedEffectPoints;
            var beginSorted = _beginSortedEffectPoints;

            while (_indexByEndSorted - 1 >= 0 &&
                   endSorted[_indexByEndSorted - 1].EndTime.ToSecond(this) > second)
            {
                _activeEffects.Add(endSorted[_indexByEndSorted-- - 1]);
            }

            foreach (var effect in _activeEffects)
            {
                effect.UpdateEffect(second, this);
            }
                        
            while (_indexByBeginSorted - 1 >= 0 &&
                   beginSorted[_indexByBeginSorted - 1].Time.ToSecond(this) > second)
            {
                _activeEffects.Remove(beginSorted[_indexByBeginSorted-- - 1]);
            }
        }

        public double ToSecond(BaseTime time)
        {
            switch (time.Unit)
            {
                case TimeUnit.Second:
                    return time.Value;
                case TimeUnit.Beat:
                    return GetSecondByBeat(time.Value);
                default:
                    throw new InvalidOperationException("unsupported time unit: " + time.Unit);
            }
        }
        
        public double ToBeat(BaseTime time)
        {
            switch (time.Unit)
            {
                case TimeUnit.Second:
                    return GetBeatBySecond(time.Value);
                case TimeUnit.Beat:
                    return time.Value;
                default:
                    throw new InvalidOperationException("unsupported time unit: " + time.Unit);
            }
        }

        private SpeedTrigger CreateSpeedTrigger(
            ITime time, 
            double second,
            double speed,
            double x)
        {
            var position = transform.position;
            Vector3 vectorPos = new((float) x, position.y, position.z);
            GameObject gameObj = Instantiate(triggerPrefab, vectorPos, Quaternion.identity);
            SpeedTrigger trigger = gameObj.AddComponent<SpeedTrigger>();
            
            trigger.Time = time;
            trigger.Second = second;
            trigger.Speed = speed;
            trigger.Position = x;
            return trigger;
        }
        
        private BpmTrigger CreateBpmTrigger(
            double second,
            double bpm,
            double beat)
        {
            float x = (float) GetPositionBySecond(second);
            var position = transform.position;
            Vector3 vectorPos = new(x, position.y, position.z);
            GameObject gameObj = Instantiate(triggerPrefab, vectorPos, Quaternion.identity);
            BpmTrigger trigger = gameObj.AddComponent<BpmTrigger>();
            
            trigger.Second = second;
            trigger.Bpm = bpm;
            trigger.Beat = beat;
            return trigger;
        }
        
        private EffectTrigger CreateEffectTrigger(
            ITime time,
            ITime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            float x = (float) GetPositionBySecond(time.ToSecond(this));
            var position = transform.position;
            Vector3 vectorPos = new(x, position.y, position.z);
            GameObject gameObj = Instantiate(triggerPrefab, vectorPos, Quaternion.identity);
            EffectTrigger trigger = gameObj.AddComponent<EffectTrigger>();
            
            trigger.Time = time;
            trigger.Duration = duration;
            trigger.TimingFunction = function;
            trigger.Property = property;
            trigger.ToState = state;
            return trigger;
        }
    }
}
