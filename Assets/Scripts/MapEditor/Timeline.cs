using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapEditor.Timestamp;
using Serialization.Data;
using UnityEngine;
using Utils;
using VisualEffect.Function;
using VisualEffect.Property;

namespace MapEditor
{
    public class Timeline : MonoBehaviour, ITimeline
    {
        private const double PlaneScale = 10.0;
        
        private readonly List<BpmTimestamp> _bpmPoints = new();
        private readonly List<SpeedTimestamp> _speedPoints = new();
        private readonly List<EffectTimestamp> _beginSortedEffectPoints = new();
        private readonly List<EffectTimestamp> _endSortedEffectPoints = new();
        
        private readonly ISet<EffectTimestamp> _activeEffects = new HashSet<EffectTimestamp>();
        private int _indexByBeginSorted;
        private int _indexByEndSorted;
        private double _lastSecond;
        
        [SerializeField]
        private GameObject beatPrefab;
        
        [SerializeField]
        private GameObject speedPrefab;
        
        [SerializeField]
        private GameObject bpmPrefab;

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
                
        public SpeedTimestamp AddSpeedPoint(ITime time, double speed)
        {
            double second = time.ToSecond(this);
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Second, second);
            double position = index < 0 ? 0 : _speedPoints[index]
                .GetPosition(second);
            
            SpeedTimestamp point = new(time, second, speed, position);
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
                ITime time = _speedPoints[i].Time;
                double second = _speedPoints[i].Second;
                double speed = _speedPoints[i].Speed;
                double position = _speedPoints[i - 1].GetPosition(second);
                _speedPoints[i] = new SpeedTimestamp(time, second, speed, position);
                // TODO move position
            }
        }

        public double GetPositionBySecond(double second)
        {
            SpeedTimestamp point = SearchSpeedPointBySecond(second);
            if (point == null) return -1;
            return point.GetPosition(second);
        }

        private SpeedTimestamp SearchSpeedPointBySecond(double second)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Time.ToSecond(this), second);
            if (index < 0) return null;
            return _speedPoints[index];
        }

        public double GetSecondByPosition(double position)
        {
            SpeedTimestamp point = SearchSpeedPointByPosition(position);
            if (point == null) return -1;
            return point.GetSecond(position);
        }
        
        private SpeedTimestamp SearchSpeedPointByPosition(double position)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Position, position);
            if (index < 0) return null;
            return _speedPoints[index];
        }
        
        public BpmTimestamp AddBpmPoint(BaseTime time, double bpm)
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
            
            BpmTimestamp point = new(second, bpm, beat);
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
                double second = _bpmPoints[i].Second;
                double bpm = _bpmPoints[i].Bpm;
                double beat = _bpmPoints[i - 1].GetBeat(second);
                _bpmPoints[i] = new BpmTimestamp(second, bpm, beat);
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
                ITime time = _speedPoints[i].Time;
                double second = time.ToSecond(this);
                double speed = _speedPoints[i].Speed;
                double position = _speedPoints[i - 1].GetPosition(second);
                _speedPoints[i] = new SpeedTimestamp(time, second, speed, position);
                // TODO move position
            }
        }
        
        public double GetBeatBySecond(double second)
        {
            BpmTimestamp point = SearchBpmPointBySecond(second);
            if (point == null) return -1;
            return point.GetBeat(second);
        }

        private BpmTimestamp SearchBpmPointBySecond(double second)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, 
                e => e.Second, second);
            if (index < 0) return null;
            return _bpmPoints[index];
        }

        public double GetSecondByBeat(double beat)
        {
            BpmTimestamp point = SearchBpmPointByBeat(beat);
            if (point == null) return -1;
            return point.GetSecond(beat);
        }

        private BpmTimestamp SearchBpmPointByBeat(double beat)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, 
                e => e.Beat, beat);
            if (index < 0) return null;
            return _bpmPoints[index];
        }

        public EffectTimestamp AddEffectPoint(
            ITime time,
            ITime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            EffectTimestamp effect = new(time, duration, function, property, state);
            
            ITime begin = effect.BeginTime;
            ITime end = effect.BeginTime + effect.Duration;
            
            double beginSecond = begin.ToSecond(this);
            double endSecond = end.ToSecond(this);
            
            int beginIndex = CollectionUtils.SearchBinaryInRangeList(_beginSortedEffectPoints, 
                e => e.BeginTime.ToSecond(this), beginSecond);
            int endIndex = CollectionUtils.SearchBinaryInRangeList(_endSortedEffectPoints, 
                e => e.EndTime.ToSecond(this), endSecond);
            
            // Добавляем эффект в общий список
            _beginSortedEffectPoints.Insert(beginIndex + 1, effect);
            _endSortedEffectPoints.Insert(endIndex + 1, effect);
            
            // Добавляем эффект в список свойства
            int prevoiusIndex = CollectionUtils.FindPrevoius(_beginSortedEffectPoints, beginIndex + 1,
                e => e.Property == property);
            int nextIndex = CollectionUtils.FindNext(_beginSortedEffectPoints, beginIndex + 1,
                e => e.Property == property);

            effect.FromState = prevoiusIndex < 0
                ? effect.Property.GetDefault()
                : _beginSortedEffectPoints[prevoiusIndex].CalculateState(beginSecond, this);
                //: _beginSortedEffectPoints[prevoiusIndex].ToState;
            if (nextIndex >= 0)
            {
                ITime nextBeginTime = _beginSortedEffectPoints[nextIndex].BeginTime;
                double nextBeginSecond = nextBeginTime.ToSecond(this);
                
                _beginSortedEffectPoints[nextIndex].FromState
                    = effect.CalculateState(nextBeginSecond, this);
                    //= effect.ToState;
            }

            return effect;
        }

        public void RemoveEffectPoint(EffectTimestamp effect)
        {
            // Убираем эффект из общего списка
            int beginIndex = _beginSortedEffectPoints.IndexOf(effect);
            int endIndex = _endSortedEffectPoints.IndexOf(effect);
            
            _beginSortedEffectPoints.RemoveAt(beginIndex);
            _endSortedEffectPoints.RemoveAt(endIndex);

            // Убираем эффект из списка свойства
            int prevoiusIndex = CollectionUtils.FindPrevoius(_beginSortedEffectPoints, beginIndex + 1,
                e => e.Property == effect.Property);
            int nextIndex = CollectionUtils.FindNext(_beginSortedEffectPoints, beginIndex - 1,
                e => e.Property == effect.Property);

            switch (prevoiusIndex)
            {
                case >= 0 when nextIndex >= 0:
                    ITime begin = _beginSortedEffectPoints[nextIndex].BeginTime;
                    double beginSecond = begin.ToSecond(this);
                    
                    _beginSortedEffectPoints[nextIndex].FromState 
                        = _beginSortedEffectPoints[prevoiusIndex].CalculateState(beginSecond, this);
                    break;
                case < 0 when nextIndex >= 0:
                    _beginSortedEffectPoints[nextIndex].FromState 
                        = effect.Property.GetDefault();
                    break;
            }
        }

        public void Move(double second)
        {
            if (second >= _lastSecond) MoveForth(second);
            else MoveBack(second);
            _lastSecond = second;
        }

        public void ResetMove()
        {
            _activeEffects.Clear();
            Move(0);
            // TODO может сразу после Move(0) устанавливать Move(target), где target - какое время надо установить
        }

        private void MoveForth(double second)
        {
            var beginSorted = _beginSortedEffectPoints;
            var endSorted = _endSortedEffectPoints;

            while (_indexByBeginSorted < beginSorted.Count &&
                   beginSorted[_indexByBeginSorted].BeginTime.ToSecond(this) <= second)
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
                   beginSorted[_indexByBeginSorted - 1].BeginTime.ToSecond(this) > second)
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
    }
}
