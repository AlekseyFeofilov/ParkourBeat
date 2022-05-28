using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class PlacedObject : MonoObject
    {
        [VisualPropertyAttribute]
        public PositionProperty Position;
        
        [VisualPropertyAttribute]
        public RotationProperty Rotation;
        
        [VisualPropertyAttribute]
        public ScaleProperty Scale;

        private void Awake()
        {
            var transform1 = transform;
            Position = new PositionProperty(transform1);
            Rotation = new RotationProperty(transform1);
            Scale = new ScaleProperty(transform1);
        }

        // Позиция
        public class PositionProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public PositionProperty(Transform transform)
            {
                _transform = transform;
            }

            public override Vector3 Value
            {
                get => _transform.position;
                set => _transform.position = value;
            }
        }
        
        // Вращение
        public class RotationProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public RotationProperty(Transform transform)
            {
                _transform = transform;
            }

            public override Vector3 Value
            {
                get => _transform.rotation.eulerAngles;
                set => _transform.rotation = Quaternion.Euler(value);
            }
        }
        
        // Масштаб
        public class ScaleProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public ScaleProperty(Transform transform)
            {
                _transform = transform;
            }

            public override Vector3 Value
            {
                get => _transform.localScale;
                set => _transform.localScale = value;
            }
        }
    }
}