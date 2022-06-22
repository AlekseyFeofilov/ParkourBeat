using UnityEngine;
using VisualEffect.Object;

namespace VisualEffect
{
    public class SampleBeatmap : MonoBehaviour
    {

        public SkyObject sky;
        public CubeObject cube;

        private void Update()
        {
            // ТЕСТ TODO
            /*switch (Time.frameCount)
            {
                case 1000:
                    cube.Position.BeginTransition(
                        cube.Position.Value + new Vector3(0, 1, 0), 0.5f, ITimingFunction.Ease);
                    sky.SkyColor.BeginTransition(
                        Color.blue, 0.5f, ITimingFunction.Linear);
                    break;
                case 2000:
                    cube.Scale.BeginTransition(
                        cube.Scale.Value * 1.25f, 0f, ITimingFunction.Linear);
                    cube.Scale.BeginTransition(
                        new Vector3(1, 1, 1), 0.3f, ITimingFunction.Linear);
                    break;
                case 2500:
                    cube.Rotation.BeginTransition(
                        new Vector3(300, 30, 0), 10f, ITimingFunction.EaseOut);
                    sky.HorizonColor.BeginTransition(
                        Color.black, 1.5f, ITimingFunction.Linear);
                    break;
                case 3500:
                    sky.SkyColor.BeginTransition(
                        Color.yellow, 5f, ITimingFunction.Linear);
                    sky.HorizonColor.BeginTransition(
                        Color.grey, 5f, ITimingFunction.Linear);
                    break;
            }*/
        }
    }
}