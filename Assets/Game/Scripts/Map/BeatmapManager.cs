using Game.Scripts.MapEditor;
using Game.Scripts.Scenes;
using UnityEngine;

namespace Game.Scripts.Map
{
    public class BeatmapManager : MonoBehaviour
    {
        public static string CurrentName { get; private set; } = "LaLiDia";

        public void LoadBeatmapInPlaymode()
        {
            BeatmapEditorContext.Reset();
            AdvancedSceneManager.LoadScene("Playmode");
        }
        
        public void LoadBeatmapInEditmode()
        {
            BeatmapEditorContext.Reset();
            AdvancedSceneManager.LoadScene("Editmode");
        }

        public void SetName(string mapName)
        {
            CurrentName = mapName;
        }
    }
}