using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Map.Timeline;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Function;
using Game.Scripts.Map.VisualEffect.Property;
using Libraries.QuickOutline.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Scripts.MapEditor.Trigger
{
    public class EffectTrigger : MonoBehaviour, IMovable
    {
        public ITimeline Timeline;
        public Dictionary<IVisualProperty, EffectTimestamp> Timestamps = new();

        public MapTime BeginTime;
        public MapTime EndTime;
        public ITimingFunction TimingFunction;

        private Transform _fromTransfrom;
        private Transform _toTransfrom;
        private Transform _cylinderTransfrom;

        public Vector3 BeginPosition
        {
            get => _fromTransfrom.position;
            set
            {
                float x = (_toTransfrom.position.x - value.x) / 2f;
                _cylinderTransfrom.position = _toTransfrom.position;
                _cylinderTransfrom.position -= new Vector3(x, 0, 0);
                _cylinderTransfrom.localScale = new Vector3(_cylinderTransfrom.localScale.x, x, _cylinderTransfrom.localScale.z);
                _fromTransfrom.position = value;
            }
        }

        public Vector3 EndPosition
        {
            get => _toTransfrom.position;
            set
            {
                float x = (value.x - _fromTransfrom.position.x) / 2f;
                _cylinderTransfrom.position = _fromTransfrom.position;
                _cylinderTransfrom.position += new Vector3(x, 0, 0);
                _cylinderTransfrom.localScale = new Vector3(_cylinderTransfrom.localScale.x, x, _cylinderTransfrom.localScale.z);
                _toTransfrom.position = value;
            }
        }

        public bool Outlined
        {
            set
            {
                if (value && !gameObject.GetComponent<OutlinedObject>())
                {
                    OutlinedObject outlined = gameObject.AddComponent<OutlinedObject>();
                    outlined.OutlineColor = Color.red;
                    outlined.OutlineMode = OutlinedObject.Mode.OutlineAll;
                    outlined.OutlineWidth = 10;   
                }
                else if (gameObject.TryGetComponent(out OutlinedObject outlined))
                {
                    Destroy(outlined);
                }
            }
        }

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        private void Start()
        {
            _fromTransfrom = transform.Find("From");
            _fromTransfrom.AddComponent<EffectTriggerBegin>();
            _toTransfrom = transform.Find("To");
            _toTransfrom.AddComponent<EffectTriggerEnd>();
            _cylinderTransfrom = transform.Find("Cylinder");

            double beginSecond = BeginTime.ToSecond(Timeline);
            float beginX = (float) Timeline.GetPositionBySecond(beginSecond);
            BeginPosition = new Vector3(beginX, BeginPosition.y, BeginPosition.z);

            double endSecond = EndTime.ToSecond(Timeline);
            float endX = (float) Timeline.GetPositionBySecond(endSecond);
            EndPosition = new Vector3(endX, EndPosition.y, EndPosition.z);
        }

        public EffectTimestamp AddProperty(IVisualProperty property, object state)
        {
            EffectTimestamp effectTimestamp 
                = Timeline.AddEffectPoint(BeginTime, EndTime, TimingFunction, property, state);
            Timestamps[property] = effectTimestamp;
            return effectTimestamp;
        }
        
        public EffectTrigger SetX(float x)
        {
            var transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(x, pos.y, pos.z);
            return this;
        }
        
        public EffectTrigger SetY(float y)
        {
            var transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x, y, pos.z);
            return this;
        }
        
        public EffectTrigger SetZ(float z)
        {
            var transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x, pos.y, z);
            return this;
        }
        
        public EffectTrigger AddX(float x)
        {
            var transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x + x, pos.y, pos.z);
            return this;
        }
        
        public EffectTrigger AddY(float y)
        {
            var transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x, pos.y + y, pos.z);
            return this;
        }
        
        public EffectTrigger AddZ(float z)
        {
            var transform1 = transform;
            Vector3 pos = transform1.position;
            transform1.position = new Vector3(pos.x, pos.y, pos.z + z);
            return this;
        }
    }
}