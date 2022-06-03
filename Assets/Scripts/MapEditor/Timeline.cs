using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

// ReSharper disable All

namespace MapEditor
{
    public class Timeline : MonoBehaviour
    {
        private const double planeScale = 10.0;
        
        private readonly List<SpeedPoint> _speedPoints = new();
        private readonly List<BpmPoint> _bpmPoints = new();

        [SerializeField]
        private GameObject beatPrefab;
        
        [SerializeField]
        private GameObject speedPrefab;
        
        [SerializeField]
        private GameObject bpmPrefab;

        public List<GameObject>[] BeatObjects; // TODO убрать

        private void Awake()
        {
            AddSpeedPoint(0, 5);
            AddSpeedPoint(1, 1);
            AddSpeedPoint(25.25, 3);
            AddSpeedPoint(28.25, 1);
            AddSpeedPoint(31.25, 3);
            AddSpeedPoint(34, 0.5);
            AddSpeedPoint(37, 5);
            AddSpeedPoint(71.72, 1);
            
            AddBpmPoint(1.85, 82);
            AddBpmPoint(48.68, 82 * 4);
            AddBpmPoint(71.72, 82 * 2);
        }

        private void Start()
        {
            Transform transform = this.transform;
            Vector3 position = transform.position;
            Vector3 localScale = transform.localScale * (float) planeScale;

            double beginPos = position.x - localScale.x / 2;
            double endPos = position.x + localScale.x / 2;

            double beginTime = GetTimeByPosition(beginPos);
            double endTime = GetTimeByPosition(endPos);

            int beginBeat = (int) Math.Floor(GetBeatByTime(beginTime));
            int endBeat = (int) Math.Ceiling(GetBeatByTime(endTime));

            BeatObjects = new List<GameObject>[endBeat - Math.Max(beginBeat, 0) + 1];
            
            for (int beat = beginBeat; beat <= endBeat; beat++)
            {
                double time = GetTimeByBeat(beat);
                double pos = GetPositionByTime(time);
                GameObject obj;
                
                if (beat >= 0) BeatObjects[beat] = new(); // TODO убрать
                
                obj = Instantiate(beatPrefab, 
                    new((float) pos, position.y + .001f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                    );
                obj.transform.parent = transform;
                
                if (beat >= 0) BeatObjects[beat].Add(obj); // TODO убрать
                
                obj = Instantiate(beatPrefab, 
                    new((float) pos, position.y + localScale.z / 2, position.z + localScale.z / 2),
                    Quaternion.identity
                    );
                obj.transform.parent = transform;
                
                if (beat >= 0) BeatObjects[beat].Add(obj); // TODO убрать
            }

            for (int i = 1; i < _speedPoints.Count; i++)
            {
                double time = _speedPoints[i].Time;
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
                double time = _bpmPoints[i].Time;
                double pos = GetPositionByTime(time);
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
                
        public void AddSpeedPoint(double time, double speed)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, e => e.Time, time);
            double position = index < 0 ? 0 : _speedPoints[index].GetPosition(time);
            SpeedPoint point = new(time, speed, position);
            _speedPoints.Insert(index + 1, point);
            RecalcSpeedPointsFromIndex(index + 2);
        }

        public void RemoveSpeedPoint(double time)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, e => e.Time, time);
            if (index < 0) return;
            _speedPoints.RemoveAt(index);
            RecalcSpeedPointsFromIndex(index);
        }

        private void RecalcSpeedPointsFromIndex(int index)
        {
            for (int i = index; i < _speedPoints.Count; i++)
            {
                double currentTime = _speedPoints[i].Time;
                double currentSpeed = _speedPoints[i].Speed;
                double currentPos = _speedPoints[i - 1].GetPosition(_speedPoints[i].Time);
                _speedPoints[i] = new(currentTime, currentSpeed, currentPos);
            }
        }

        public double GetPositionByTime(double time)
        {
            SpeedPoint point = SearchSpeedPointByTime(time);
            if (point == null) return -1;
            return point.GetPosition(time);
        }

        private SpeedPoint SearchSpeedPointByTime(double time)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, e => e.Time, time);
            if (index < 0) return null;
            return _speedPoints[index];
        }

        public double GetTimeByPosition(double position)
        {
            SpeedPoint point = SearchSpeedPointByPosition(position);
            if (point == null) return -1;
            return point.GetTime(position);
        }
        
        private SpeedPoint SearchSpeedPointByPosition(double position)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_speedPoints, e => e.Position, position);
            if (index < 0) return null;
            return _speedPoints[index];
        }
        
        public void AddBpmPoint(double time, double bpm)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, e => e.Time, time);
            double beat = index < 0 ? 0 : _bpmPoints[index].GetBeat(time);
            BpmPoint point = new(time, bpm, beat);
            _bpmPoints.Insert(index + 1, point);
            RecalcBpmPointsFromIndex(index + 2);
        }

        public void RemoveBpmPoint(double time)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, e => e.Time, time);
            if (index < 0) return;
            _bpmPoints.RemoveAt(index);
            RecalcBpmPointsFromIndex(index);
        }

        private void RecalcBpmPointsFromIndex(int index)
        {
            for (int i = index; i < _bpmPoints.Count; i++)
            {
                double currentTime = _bpmPoints[i].Time;
                double currentBpm = _bpmPoints[i].Bpm;
                double currentBeat = _bpmPoints[i - 1].GetBeat(_speedPoints[i].Time);
                _bpmPoints[i] = new(currentTime, currentBpm, currentBeat);
            }
        }
        
        public double GetBeatByTime(double time)
        {
            BpmPoint point = SearchBpmPointByTime(time);
            if (point == null) return -1;
            return point.GetBeat(time);
        }

        private BpmPoint SearchBpmPointByTime(double time)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, (e) => e.Time, time);
            if (index < 0) return null;
            return _bpmPoints[index];
        }

        public double GetTimeByBeat(double beat)
        {
            BpmPoint point = SearchBpmPointByBeat(beat);
            if (point == null) return -1;
            return point.GetTime(beat);
        }

        private BpmPoint SearchBpmPointByBeat(double beat)
        {
            int index = CollectionUtils.SearchBinaryInRangeList(_bpmPoints, (e) => e.Beat, beat);
            if (index < 0) return null;
            return _bpmPoints[index];
        }
    }

    public class SpeedPoint
    {
        public readonly double Time;
        public readonly double Speed;
        public readonly double Position;

        public SpeedPoint(double time, double speed, double position)
        {
            Time = time;
            Speed = speed;
            Position = position;
        }

        public double GetTime(double position)
        {
            double x = position - Position;
            double y = x / Speed;
            return y + Time;
        }

        public double GetPosition(double time)
        {
            double x = time - Time;
            double y = x * Speed;
            return y + Position;
        }
    }
    
    public class BpmPoint
    {
        public readonly double Time;
        public readonly double Bpm;
        public readonly double Beat;

        public BpmPoint(double time, double bpm, double beat)
        {
            Time = time;
            Bpm = bpm;
            Beat = beat;
        }

        public double GetTime(double beat)
        {
            double x = beat - Beat;
            double y = x / Bpm * 60;
            return y + Time;
        }

        public double GetBeat(double time)
        {
            double x = time - Time;
            double y = x * Bpm / 60;
            return y + Beat;
        }
    }
}
