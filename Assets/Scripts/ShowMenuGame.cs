using UnityEngine;

namespace DefaultNamespace
{
    public class ShowMenuGame : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
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
        }


        public void HideMenu()
        {
            menu.SetActive(false);
            _activeMenu = false;
        }
    }
}