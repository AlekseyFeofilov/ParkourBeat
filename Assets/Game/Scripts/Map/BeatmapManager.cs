using Game.Scripts.MapEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Map
{
    public class BeatmapManager : MonoBehaviour
    {
        public static string CurrentName { get; private set; } = "LaLiDia";

        public void LoadBeatmapInPlaymode()
        {
            BeatmapEditorContext.Reset();
            SceneManager.LoadScene("Playmode");
        }
        
        public void LoadBeatmapInEditmode()
        {
            BeatmapEditorContext.Reset();
            SceneManager.LoadScene("Editmode");
        }

        public void SetName(string mapName)
        {
            CurrentName = mapName;
        }
    }
}