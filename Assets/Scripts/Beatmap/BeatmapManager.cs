using MapEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Beatmap
{
    public class BeatmapManager : MonoBehaviour
    {
        public static string CurrentName { get; private set; } = "Example";

        public void LoadBeatmapInPlaymode()
        {
            BeatmapEditorContext.Reset();
            SceneManager.LoadScene("Map Playmode");
        }
        
        public void LoadBeatmapInEditmode()
        {
            BeatmapEditorContext.Reset();
            SceneManager.LoadScene("Map Editmode");
        }

        public void SetName(string mapName)
        {
            CurrentName = mapName;
        }
    }
}