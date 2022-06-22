using MapEditor;
using MapEditor.ChangeableInterfaces;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class CubeObject : MonoObject, IMovable, IRotatable, IScalable, IColorable
    {
        [VisualPropertyAttribute(Id = "Position")]
        public PositionProperty Position;
        
        [VisualPropertyAttribute(Id = "Rotation")]
        public RotationProperty Rotation;
        
        [VisualPropertyAttribute(Id = "Scale")]
        public ScaleProperty Scale;
        
        [VisualPropertyAttribute(Id = "Color")]
        public ColorProperty Color;

        private void Awake()
        {
            Renderer renderer = GetComponent<Renderer>();
            Transform transform1 = transform;
            
            Position = new PositionProperty(transform1);
            Rotation = new RotationProperty(transform1);
            Scale = new ScaleProperty(transform1);
            Color = new ColorProperty(renderer.material);
        }
        
        public bool OnEndMove()
        {
            BeatmapEditorContext.SetPropertyValue(Position, transform.position);
            return true;
        }

        public bool OnEndRotate()
        {
            BeatmapEditorContext.SetPropertyValue(Rotation, transform.rotation.eulerAngles);
            return true;
        }

        public bool OnEndScale()
        {
            BeatmapEditorContext.SetPropertyValue(Scale, transform.localScale);
            return true;
        }

        public bool OnChange(Color color)
        {
            BeatmapEditorContext.SetPropertyValue(Color, color);
            return true;
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
        
        // Цвет
        public class ColorProperty : AbstractColorProperty
        {
            private readonly Material _material;

            public ColorProperty(Material material)
            {
                _material = material;
            }

            protected override void Apply(Color state)
            {
                _material.color = state;
            }
        }
    }
}