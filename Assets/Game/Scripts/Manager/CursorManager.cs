using UnityEngine;

namespace Game.Scripts.Manager
{
    public class CursorManager : MonoBehaviour
    {
        private void OnEnable()
        {
            Hide();
        }

        private void OnDisable()
        {
            Show();
        }

        public void Hide()
        {
            Cursor.visible = false;
        }

        public void Show()
        {
            Cursor.visible = true;
        }
    }
}