using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.SettingsScene
{
    public class ToggleListener : MonoBehaviour
    {
        public void SetOffAA(Toggle change)
        {
            if (!change.isOn) return;
            GraphicsManager.CurrentMode = GraphicsManager.Mode.Off;
        }

        public void SetFXAA(Toggle change)
        {
            if (!change.isOn) return;
            GraphicsManager.CurrentMode = GraphicsManager.Mode.FXAA;
        }

        public void SetSMAA(Toggle change)
        {
            if (!change.isOn) return;
            GraphicsManager.CurrentMode = GraphicsManager.Mode.SMAA;
        }

        public void SetTAA(Toggle change)
        {
            if (!change.isOn) return;
            GraphicsManager.CurrentMode = GraphicsManager.Mode.TAA;
        }
    }
}