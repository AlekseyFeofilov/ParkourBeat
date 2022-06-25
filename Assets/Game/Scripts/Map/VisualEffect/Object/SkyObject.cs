using Game.Scripts.Engine.Api.Event;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Manager;
using Game.Scripts.Map.VisualEffect.Property;
using Game.Scripts.MapEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Map.VisualEffect.Object
{
    public class SkyObject : MonoObject, ISelectable, IColorable
    {
        [SerializeField] private Skybox skybox;
        [FormerlySerializedAs("mainTools")] [SerializeField] private ToolManager toolManager;
        
        [VisualProperty(Id = "Color")]
        public SkyColorProperty Color;

        private void Awake()
        {
            Color = new SkyColorProperty(skybox);
        }

        public void OnSelect(SelectEvent @event)
        {
            toolManager.ToolMode = ToolManager.Mode.ColorTool;
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

        // Цвет неба
        public class SkyColorProperty : AbstractColorProperty
        {
            private static readonly int SkyColor = Shader.PropertyToID("_SkyColor");
            private readonly Skybox _skybox;

            public SkyColorProperty(Skybox skybox)
            {
                _skybox = skybox;
            }

            protected override void Apply(Color state)
            {
                _skybox.material.SetColor(SkyColor, state);
            }

            public Color Get()
            {
                return _skybox.material.GetColor(SkyColor);
            }
        }
    }
}
