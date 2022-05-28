using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;

namespace VisualEffect
{
    public class SampleBeatmap : MonoBehaviour
    {

        public SkyObject Sky;
        public PlacedObject Cube;

        private void Update()
        {
            // ТЕСТ
            switch (Time.frameCount)
            {
                case 1000:
                    Cube.Position.BeginTransition(Cube.Position.Value + new Vector3(0, 1, 0), 0.5f, ITimingFunction.Ease);
                    Sky.SkyColor.BeginTransition(Color.blue, 0.5f, ITimingFunction.Linear);
                    break;
                case 2000:
                    Cube.Scale.BeginTransition(Cube.Scale.Value * 1.25f, 0f, ITimingFunction.Linear);
                    Cube.Scale.BeginTransition(new Vector3(1, 1, 1), 0.3f, ITimingFunction.Linear);
                    break;
                case 2500:
                    Cube.Rotation.BeginTransition(new Vector3(300, 30, 0), 10f, ITimingFunction.EaseOut);
                    Sky.HorizonColor.BeginTransition(Color.black, 1.5f, ITimingFunction.Linear);
                    break;
                case 3500:
                    Sky.SkyColor.BeginTransition(Color.yellow, 5f, ITimingFunction.Linear);
                    Sky.HorizonColor.BeginTransition(Color.grey, 5f, ITimingFunction.Linear);
                    break;
            }
        }
    }
}