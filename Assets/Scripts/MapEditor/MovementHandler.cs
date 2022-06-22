using MapEditor.Utils;
using UnityEngine;

namespace MapEditor
{
    public class MovementHandler : MonoBehaviour
    {
        private bool _activated;
        private Vector3 _begin;
        private Vector3 _last;
        
        private void LateUpdate()
        {
            IMovable movable;
            if (!MainSelect.SelectedObj) return;
            if ((movable = MainSelect.SelectedObj.gameObject.GetComponent<IMovable>()) == null) return;
            Vector3 position = MainSelect.SelectedObj.transform.position;
            
            if (Input.GetMouseButton(0))
            {
                if (!_activated)
                {
                    if (!movable.OnBeginMove()) return;
                    _begin = position;
                }
                else if (!movable.OnMove())
                {
                    MainSelect.SelectedObj.transform.position = _last;
                }

                _last = position;
                _activated = true;
            }
            else if (_activated)
            {
                if (!movable.OnEndMove())
                {
                    MainSelect.SelectedObj.transform.position = _begin;
                }
                _activated = false;
            }
        }
    }
}