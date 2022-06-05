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

            protected override void Apply(Vector3 state)
            {
                _transform.position = state;
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

            protected override void Apply(Vector3 state)
            {
                _transform.rotation = Quaternion.Euler(state);
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

            protected override void Apply(Vector3 state)
            {
                _transform.localScale = state;
            }
        }
    }
}