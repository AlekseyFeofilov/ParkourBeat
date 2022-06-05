using System;
using System.Collections.Generic;
using MapEditor.Timestamp;
using Serialization.Data;
using UnityEngine;
using Utils;
using VisualEffect.Point;

// ReSharper disable All

namespace MapEditor
{
    public class Timeline : MonoBehaviour, ITimeConverter
    {
        private const double planeScale = 10.0;
        
        private readonly List<BpmTimestamp> _bpmPoints = new();
        private readonly List<SpeedTimestamp> _speedPoints = new();
        private readonly List<VisualEffectPoint> _beginSortedEffectPoints = new();
        private readonly List<VisualEffectPoint> _endSortedEffectPoints = new();
        
        private readonly ISet<VisualEffectPoint> _activeEffects = new HashSet<VisualEffectPoint>();
        private int _indexByBeginSorted;
        private int _indexByEndSorted;
        private double _lastSecond;
        
        [SerializeField]
        private GameObject beatPrefab;
        
        [SerializeField]
        private GameObject speedPrefab;
        
        [SerializeField]
        private GameObject bpmPrefab;

        private void Start()
        {
            Transform transform = this.transform;
            Vector3 position = transform.position;
            Vector3 localScale = transform.localScale * (float) planeScale;

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
                    new((float) pos, position.y + .001f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                    );
                obj.transform.parent = transform;

                obj = Instantiate(beatPrefab, 
                    new((float) pos, position.y + localScale.z / 2, position.z + localScale.z / 2),
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
                    new((float) pos, position.y + .002f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                );
                obj.transform.parent = transform;
                
                obj = Instantiate(speedPrefab, 
                    new((float) pos, position.y + localScale.z / 2, position.z + localScale.z / 2),
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
                    new((float) pos, position.y + .003f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                );
                obj.transform.parent = transform;
                
                obj = Instantiate(bpmPrefab, 
                    new((float) pos, position.y + localScale.z / 2, position.z + localScale.z / 2),
                    Quaternion.identity
                );
                obj.transform.parent = transform;
            }
        }
                
        public void AddSpeedPoint(ITime time, double speed)
        {
            double second = time.ToSecond(this);
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, 
                e => e.Second, second);
            double position = index < 0 ? 0 : _speedPoints[index]
                .GetPosition(second);
            
            SpeedTimestamp point = new(time, second, speed, position);
            _speedPoints.Insert(index + 1, point);
            RecalcSpeedPointsFromIndex(index + 2);
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
                ITime currentTime = _speedPoints[i].Time;
                double currentSecond = _speedPoints[i].Second;
                double currentSpeed = _speedPoints[i].Speed;
                double currentPos = _speedPoints[i - 1]
                    .GetPosition(currentSecond);
                _speedPoints[i] = new(currentTime, currentSecond, 
                    currentSpeed, currentPos);
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
        
        public void AddBpmPoint(BaseTime time, double bpm)
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
                double currentBpm = _bpmPoints[i].Bpm;
                double currentBeat = _bpmPoints[i - 1]
                    .GetBeat(currentSecond);
                _bpmPoints[i] = new(currentSecond, currentBpm, currentBeat);
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
                double currentSpeed = _speedPoints[i].Speed;
                double currentPos = _speedPoints[i - 1]
                    .GetPosition(currentSecond);
                _speedPoints[i] = new(currentTime, currentSecond, 
                    currentSpeed, currentPos);
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

        public void AddEffectPoint(VisualEffectPoint effect)
        {
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
        }

        public void RemoveEffectPoint(VisualEffectPoint effect)
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
                if (_indexByBeginSorted >= beginSorted.Count)
                {
                    _indexByBeginSorted--;
                    break;
                }
            }

            foreach (var effect in _activeEffects)
            {
                effect.Update(second, this);
            }

            while (_indexByEndSorted < endSorted.Count &&
                   endSorted[_indexByEndSorted].EndTime.ToSecond(this) <= second)
            {
                _activeEffects.Remove(endSorted[_indexByEndSorted++]);
                if (_indexByEndSorted >= endSorted.Count)
                {
                    _indexByEndSorted--;
                    break;
                }
            }
        }
        
        private void MoveBack(double second)
        {
            var endSorted = _endSortedEffectPoints;
            var beginSorted = _beginSortedEffectPoints;

            while (_indexByEndSorted >= 0 &&
                   endSorted[_indexByEndSorted].EndTime.ToSecond(this) > second)
            {
                _activeEffects.Add(endSorted[_indexByEndSorted--]);
                if (_indexByEndSorted < 0)
                {
                    _indexByEndSorted++;
                    break;
                }
            }

            foreach (var effect in _activeEffects)
            {
                effect.Update(second, this);
            }
                        
            while (_indexByBeginSorted >= 0 &&
                   beginSorted[_indexByBeginSorted].Time.ToSecond(this) > second)
            {
                _activeEffects.Remove(beginSorted[_indexByBeginSorted--]);
                if (_indexByBeginSorted < 0)
                {
                    _indexByBeginSorted++;
                    break;
                }
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
