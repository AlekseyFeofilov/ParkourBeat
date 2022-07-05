using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Settings.GraphicsSettings
{
    public class ToggleListener : MonoBehaviour
    {
        private void SetAAMode(Toggle change, GraphicsManager.Mode mode)
        {
            if (!change.isOn) return;
            GraphicsManager.CurrentMode = mode;
            ToggleGroupManager.ActiveToggle = change;
        }

        public void SetOffAA(Toggle change) => SetAAMode(change, GraphicsManager.Mode.Off);

        public void SetFXAA(Toggle change) => SetAAMode(change, GraphicsManager.Mode.FXAA);

        public void SetSMAA(Toggle change) => SetAAMode(change, GraphicsManager.Mode.SMAA);

        public void SetTAA(Toggle change) => SetAAMode(change, GraphicsManager.Mode.TAA);
    }
}