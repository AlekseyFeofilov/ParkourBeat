using System;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class SkyObject : MonoObject
    {
        [VisualPropertyAttribute]
        private SkyColorProperty _skyColor;
        
        [VisualPropertyAttribute]
        private HorizonColorProperty _horizonColor;
        
        private void Awake()
        {
            var skybox = gameObject.GetComponent<Skybox>();
            _skyColor = new SkyColorProperty(skybox);
            _horizonColor = new HorizonColorProperty(skybox);
        }
        
        private void Update()
        {
            // ТЕСТ
            switch (Time.frameCount)
            {
                case 1000:
                    _skyColor.BeginTransition(Color.blue, 0.5f, ITimingFunction.Linear);
                    break;
                case 2500:
                    _horizonColor.BeginTransition(Color.black, 2f, ITimingFunction.Linear);
                    break;
                case 3000:
                    _skyColor.BeginTransition(Color.yellow, 5f, ITimingFunction.Linear);
                    _horizonColor.BeginTransition(Color.grey, 5f, ITimingFunction.Linear);
                    break;
            }
        }
        
        // Цвет неба
        private class SkyColorProperty : AbstractColorProperty
        {
            private static readonly int SkyColor = Shader.PropertyToID("_SkyColor");
            private readonly Skybox _skybox;

            public SkyColorProperty(Skybox skybox)
            {
                _skybox = skybox;
            }

            public override Color Value
            {
                get => _skybox.material.GetColor(SkyColor);
                set => _skybox.material.SetColor(SkyColor, value);
            }
        }
        
        // Цвет горизонта
        private class HorizonColorProperty : AbstractColorProperty
        {
            private static readonly int HorizonColor = Shader.PropertyToID("_HorizonColor");
            private readonly Skybox _skybox;

            public HorizonColorProperty(Skybox skybox)
            {
                _skybox = skybox;
            }

            public override Color Value
            {
                get => _skybox.material.GetColor(HorizonColor);
                set => _skybox.material.SetColor(HorizonColor, value);
            }
        }
    }
}
