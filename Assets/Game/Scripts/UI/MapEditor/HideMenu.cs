using JetBrains.Annotations;
using UnityEngine;

namespace Game.Scripts.UI.MapEditor
{
    public class HideMenu : MonoBehaviour
    {
        public GameObject menu;
        [CanBeNull] public GameObject button;

        public void MenuHide()
        {
            menu.SetActive(false);
            if (button != null)
            {
                button.SetActive(true);
            }
        }
    }
}