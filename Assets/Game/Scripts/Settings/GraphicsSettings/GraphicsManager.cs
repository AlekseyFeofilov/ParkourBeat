namespace Game.Scripts.Settings.GraphicsSettings
{
    public static class GraphicsManager
    {
        public enum Mode
        {
            Off,
            FXAA,
            SMAA,
            TAA,
        }

        public static Mode CurrentMode = Mode.TAA;
    }
}