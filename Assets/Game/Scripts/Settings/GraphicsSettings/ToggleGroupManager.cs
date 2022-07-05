using UnityEngine.UI;

namespace Game.Scripts.Settings.GraphicsSettings
{
    public class ToggleGroupManager : ToggleGroup
    {
        public static Toggle ActiveToggle;
        
        protected override void Start()
        {
            base.Start();
            if(!ActiveToggle) return;
            
            ActiveToggle.isOn = true;
        }
    }
}