using Game.Scripts.Manager;
using Game.Scripts.Map;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.UI
{
    public class ShowMenuGame : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private PlayableBeatmap beatmap;
        [FormerlySerializedAs("hideCursorScript")] [SerializeField] private CursorManager cursorManager;
        
        private bool _activeMenu = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_activeMenu)
            {
                ShowMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && _activeMenu)
            {
                HideMenu();
            }
        }

        private void ShowMenu()
        {
            menu.SetActive(true);
            _activeMenu = true;
            if (beatmap != null)
            {
                beatmap.PauseAudio();
            }

            if (cursorManager != null)
            {
                cursorManager.Show();
            }
        }


        public void HideMenu()
        {
            menu.SetActive(false);
            _activeMenu = false;
            if (beatmap != null)
            {
                beatmap.ContinueAudio();
            }
        }
    }
}