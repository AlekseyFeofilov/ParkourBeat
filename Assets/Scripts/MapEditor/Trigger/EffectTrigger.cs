using System.Diagnostics.CodeAnalysis;
using MapEditor.Timestamp;
using MapEditor.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace MapEditor.Trigger
{
    public class EffectTrigger : MonoBehaviour, IMovable
    {
        public ITimeline Timeline;
        public EffectTimestamp Timestamp;
        
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

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        private void Start()
        {
            _fromTransfrom = transform.Find("From");
            _fromTransfrom.AddComponent<EffectTriggerBegin>();
            _toTransfrom = transform.Find("To");
            _toTransfrom.AddComponent<EffectTriggerEnd>();
            _cylinderTransfrom = transform.Find("Cylinder");

            double beginSecond = Timestamp.BeginTime.ToSecond(Timeline);
            float beginX = (float) Timeline.GetPositionBySecond(beginSecond);
            BeginPosition = new Vector3(beginX, BeginPosition.y, BeginPosition.z);

            double endSecond = Timestamp.EndTime.ToSecond(Timeline);
            float endX = (float) Timeline.GetPositionBySecond(endSecond);
            EndPosition = new Vector3(endX, EndPosition.y, EndPosition.z);
        }

        public EffectTrigger SetX(float x)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(x, pos.y, pos.z);
            return this;
        }
        
        public EffectTrigger SetY(float y)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, y, pos.z);
            return this;
        }
        
        public EffectTrigger SetZ(float z)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, z);
            return this;
        }
        
        public EffectTrigger AddX(float x)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x + x, pos.y, pos.z);
            return this;
        }
        
        public EffectTrigger AddY(float y)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y + y, pos.z);
            return this;
        }
        
        public EffectTrigger AddZ(float z)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, pos.z + z);
            return this;
        }
    }
}