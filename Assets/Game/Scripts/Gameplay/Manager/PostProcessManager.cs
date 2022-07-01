using System;
using Game.Scripts.UI.SettingsScene;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Game.Scripts.Gameplay.Manager
{
    public class PostProcessManager : MonoBehaviour
    {
        private PostProcessLayer _postProcessLayer;
        
        private void Start()
        {
            _postProcessLayer = Camera.main!.GetComponent<PostProcessLayer>();
            SetAntiAliasingMode();
        }

        private void SetAntiAliasingMode()
        {
            _postProcessLayer.antialiasingMode = GraphicsManager.CurrentMode switch
            {
                GraphicsManager.Mode.Off => PostProcessLayer.Antialiasing.None,
                GraphicsManager.Mode.FXAA => PostProcessLayer.Antialiasing.FastApproximateAntialiasing,
                GraphicsManager.Mode.SMAA => PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing,
                GraphicsManager.Mode.TAA => PostProcessLayer.Antialiasing.TemporalAntialiasing,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}