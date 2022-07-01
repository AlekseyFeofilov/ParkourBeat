using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Game.Scripts.MapEditor.Timeline
{
    public class TimelineDisplay : MonoBehaviour
    {
        private const double PlaneScale = 10.0;
        
        [SerializeField] private Map.Timeline.Timeline timeline;
        [SerializeField] private GameObject beatPrefab;
        [SerializeField] private GameObject speedPrefab;
        [SerializeField] private GameObject bpmPrefab;
        
        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
        
        public void Draw()
        {
            Transform transform = this.transform;
            Vector3 position = transform.position;
            Vector3 localScale = transform.localScale * (float) PlaneScale;

            double beginPos = position.x - localScale.x / 2;
            double endPos = position.x + localScale.x / 2;

            double beginTime = timeline.GetSecondByPosition(beginPos);
            double endTime = timeline.GetSecondByPosition(endPos);

            int beginBeat = (int) Math.Floor(timeline.GetBeatBySecond(beginTime));
            int endBeat = (int) Math.Ceiling(timeline.GetBeatBySecond(endTime));

            for (int beat = beginBeat; beat <= endBeat; beat++)
            {
                double time = timeline.GetSecondByBeat(beat);
                double pos = timeline.GetPositionBySecond(time);
                GameObject obj;

                obj = Instantiate(beatPrefab, 
                    new Vector3((float) pos, position.y + .001f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                    );
                obj.transform.parent = transform;
            }

            for (int i = 1; i < timeline.SpeedPoints.Count; i++)
            {
                double time = timeline.SpeedPoints[i].Time.ToSecond(timeline);
                double pos = timeline.SpeedPoints[i - 1].GetPosition(time);
                GameObject obj;
                
                obj = Instantiate(speedPrefab, 
                    new Vector3((float) pos, position.y + .002f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                );
                obj.transform.parent = transform;
            }
            
            for (int i = 1; i < timeline.BpmPoints.Count; i++)
            {
                double second = timeline.BpmPoints[i].Second;
                double pos = timeline.GetPositionBySecond(second);
                GameObject obj;
                
                obj = Instantiate(bpmPrefab, 
                    new Vector3((float) pos, position.y + .003f, position.z), 
                    Quaternion.Euler(90, 0, 0)
                );
                obj.transform.parent = transform;
            }
        }
    }
}