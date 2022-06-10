using MapEditor;
using MapEditor.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    public class MovementHandler : MonoBehaviour
    {
        private bool _activated;
        
        private void LateUpdate()
        {
            IMovable movable;
            if (!MainSelect.SelectedObj) return;
            if ((movable = MainSelect.SelectedObj.gameObject.GetComponent<IMovable>()) == null) return;
            
            if (Input.GetMouseButton(0))
            {
                if (!_activated) movable.OnBeginMove();
                else movable.OnMove();
                _activated = true;
            }
            else if (_activated)
            {
                movable.OnEndMove();
                _activated = false;
            }
        }
    }
}