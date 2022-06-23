using MapEditor;
using MapEditor.ChangeableInterfaces;
using MapEditor.Event;
using MapEditor.Select;
using MapEditor.Tools;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class HorizonObject : MonoObject, ISelectable, IColorable
    {
        [SerializeField] private Skybox skybox;
        [SerializeField] private MainTools mainTools;

        [VisualPropertyAttribute(Id = "Color")]
        public HorizonColorProperty Color;

        private void Awake()
        {
            Color = new HorizonColorProperty(skybox);
        }

        public void OnSelect(SelectEvent @event)
        {
            mainTools.ToolMode = MainTools.Mode.ColorTool;
        }

        public bool OnChange(Color color)
        {
            BeatmapEditorContext.SetPropertyValue(Color, color);
            Color.Apply(color);
            return false;
        }

        public void OnBeginColor(ColorBeginEvent @event)
        {
            @event.StartColor = Color.Get();
        }

        // Цвет горизонта
        public class HorizonColorProperty : AbstractColorProperty
        {
            private static readonly int HorizonColor = Shader.PropertyToID("_HorizonColor");
            private readonly Skybox _skybox;

            public HorizonColorProperty(Skybox skybox)
            {
                _skybox = skybox;
            }

            protected override void Apply(Color state)
            {
                _skybox.material.SetColor(HorizonColor, state);
                RenderSettings.fogColor = state;
            }

            public Color Get()
            {
                return _skybox.material.GetColor(HorizonColor);
            }
        }
    }
}
