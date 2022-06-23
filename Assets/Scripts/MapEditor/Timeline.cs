using System;
using System.Collections.Generic;
using System.Linq;
using Beatmap.Object;
using DataStructures.BiDictionary;
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
        private readonly List<BpmTimestamp> _bpmPoints = new();
        private readonly List<SpeedTimestamp> _speedPoints = new();
        private readonly List<EffectTimestamp> _beginSortedEffectPoints = new();
        private readonly List<EffectTimestamp> _endSortedEffectPoints = new();
        
        private readonly System.Collections.Generic.ISet<EffectTimestamp> _activeEffects = new HashSet<EffectTimestamp>();
        private int _indexByBeginSorted;
        private int _indexByEndSorted;
        private double _lastSecond;
        private bool _changed;

        [SerializeField]
        private ObjectManager objectManager;

        public List<SpeedTimestamp> SpeedPoints => _speedPoints;
        public List<BpmTimestamp> BpmPoints => _bpmPoints;

        public SpeedTimestamp AddSpeedPoint(MapTime time, double speed)
        {
            double second = time.ToSecond(this);
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Second, second);
            double position = index < 0 ? 0 : _speedPoints[index]
                .GetPosition(second);
            
            SpeedTimestamp point = new(time, second, speed, position);
            _speedPoints.Insert(index + 1, point);
            RecalcSpeedPointsFromIndex(index + 2);
            _changed = true;
            return point;
        }

        public void RemoveSpeedPoint(MapTime time)
        {
            double second = time.ToSecond(this);
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Second, second);
            if (index < 0) return;
            
            _speedPoints.RemoveAt(index);
            RecalcSpeedPointsFromIndex(index);
            _changed = true;
        }

        private void RecalcSpeedPointsFromIndex(int index)
        {
            for (int i = index; i < _speedPoints.Count; i++)
            {
                MapTime time = _speedPoints[i].Time;
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
        
        public BpmTimestamp AddBpmPoint(MapTime time, double bpm)
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
            _changed = true;
            return point;
        }

        public void RemoveBpmPoint(MapTime time)
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
            _changed = true;
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
                MapTime time = _speedPoints[i].Time;
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
            MapTime beginTime,
            MapTime endTime,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            EffectTimestamp effect = new(beginTime, endTime, function, property, state);
            
            MapTime begin = effect.BeginTime;
            MapTime end = effect.EndTime;
            
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
                MapTime nextBeginTime = _beginSortedEffectPoints[nextIndex].BeginTime;
                double nextBeginSecond = nextBeginTime.ToSecond(this);
                
                _beginSortedEffectPoints[nextIndex].FromState
                    = effect.CalculateState(nextBeginSecond, this);
                    //= effect.ToState;
            }
            _changed = true;
            
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
            int prevoiusIndex = CollectionUtils.FindPrevoius(_beginSortedEffectPoints, beginIndex,
                e => e.Property == effect.Property);
            int nextIndex = CollectionUtils.FindNext(_beginSortedEffectPoints, beginIndex - 1,
                e => e.Property == effect.Property);

            switch (prevoiusIndex)
            {
                case >= 0 when nextIndex >= 0:
                    MapTime begin = _beginSortedEffectPoints[nextIndex].BeginTime;
                    double beginSecond = begin.ToSecond(this);
                    
                    _beginSortedEffectPoints[nextIndex].FromState 
                        = _beginSortedEffectPoints[prevoiusIndex].CalculateState(beginSecond, this);
                    break;
                case < 0 when nextIndex >= 0:
                    _beginSortedEffectPoints[nextIndex].FromState 
                        = effect.Property.GetDefault();
                    break;
            }
            _changed = true;
        }
        
        public void RemoveVisualProperty(IVisualProperty property)
        {
            List<EffectTimestamp> list = new();
            foreach (var effect in _beginSortedEffectPoints)
            {
                if (effect.Property == property)
                {
                    list.Add(effect);
                }
            }

            foreach (var effectTimestamp in list)
            {
                RemoveEffectPoint(effectTimestamp);
            }
        }

        public void UpdateDefault(IVisualProperty property, object state)
        {
            property.SetDefault(state);
            if (_beginSortedEffectPoints.Count > 0)
            {
                _beginSortedEffectPoints[0].FromState = state;
            }
        }

        public void UpdateEffect(EffectTimestamp effect, object state)
        {
            int beginIndex = _beginSortedEffectPoints.IndexOf(effect);
            
            int nextIndex = CollectionUtils.FindNext(_beginSortedEffectPoints, beginIndex,
                e => e.Property == effect.Property);

            effect.ToState = state;
            if (nextIndex >= 0)
            {
                _beginSortedEffectPoints[nextIndex].FromState = state;
            }
        }

        public BiDictionary<int, EffectTimestamp> GetEffectPointIdDictonary()
        {
            BiDictionary<int, EffectTimestamp> dictionary = new();
            for (int i = 0; i < _beginSortedEffectPoints.Count; i++)
            {
                dictionary.Add(i, _beginSortedEffectPoints[i]);
            }

            return dictionary;
        }

        public void Move(double second)
        {
            if (_changed) ResetMove();
            if (second >= _lastSecond) MoveForth(second);
            else MoveBack(second);
            _lastSecond = second;
            _changed = false;
        }

        public void ResetMove()
        {
            _activeEffects.Clear();
            _indexByBeginSorted = 0;
            _indexByEndSorted = 0;
            foreach (var obj in objectManager.Objects)
            {
                obj.Reset();
            }
            /*if (_lastSecond <= 0) MoveForth(0);
            else MoveBack(0);*/
            _lastSecond = 0;
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

        public double ToSecond(MapTime time)
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
        
        public double ToBeat(MapTime time)
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

        public void SaveData(BeatmapData data)
        {
            data.Bpm = (from point in _bpmPoints
                select new BpmTimestampData
                {
                    Second = point.Second,
                    Bpm = point.Bpm,
                    Beat = point.Beat
                }).ToList();
            
            data.Speed = (from point in _speedPoints
                select new SpeedTimestampData
                {
                    Time = point.Time,
                    Second = point.Second,
                    Speed = point.Speed,
                    Position = point.Position
                }).ToList();
            
            Dictionary<EffectTimestamp, int> effects = new();
            for (int i = 0; i < _beginSortedEffectPoints.Count; i++)
            {
                data.BeginSortedEffects.Add(new EffectTimestampData
                {
                    Index = i,
                    BeginTime = _beginSortedEffectPoints[i].BeginTime,
                    EndTime = _beginSortedEffectPoints[i].EndTime,
                    TimingFunction = _beginSortedEffectPoints[i].TimingFunction,
                    Property = objectManager.GetIdByProperty(_beginSortedEffectPoints[i].Property),
                    FromState = new ValueData(_beginSortedEffectPoints[i].FromState),
                    ToState = new ValueData(_beginSortedEffectPoints[i].ToState),
                });
                effects[_beginSortedEffectPoints[i]] = i;
            }

            foreach (var t in _endSortedEffectPoints)
            {
                data.EndSortedEffects.Add(effects[t]);
            }
        }

        public void LoadData(BeatmapData data)
        {
            _bpmPoints.Clear();
            _bpmPoints.AddRange(from point in data.Bpm
                select new BpmTimestamp(point.Second, point.Bpm, point.Beat));
            
            _speedPoints.Clear();
            _speedPoints.AddRange(from point in data.Speed
                select new SpeedTimestamp(point.Time, point.Second, point.Speed, point.Position));
            
            _beginSortedEffectPoints.Clear();
            _beginSortedEffectPoints.AddRange(from point in data.BeginSortedEffects
                select new EffectTimestamp(point.BeginTime, point.EndTime)
                {
                    TimingFunction = point.TimingFunction, 
                    Property = objectManager.GetPropertyById(point.Property), 
                    ToState = point.ToState.Value,
                    FromState = point.FromState.Value
                });
            
            _endSortedEffectPoints.Clear();
            _endSortedEffectPoints.AddRange(from index in data.EndSortedEffects
                select _beginSortedEffectPoints[index]);
 
            ResetMove();
        }
    }
}
