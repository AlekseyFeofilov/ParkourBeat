using UnityEngine;

namespace Game.Scripts.UI.MapEditor
{
    public class ShowMenu : MonoBehaviour
    {
        public GameObject menu;
        public GameObject button;

        public void MenuShow()
        {
            menu.SetActive(true);
            button.SetActive(false);
        }
    }
}