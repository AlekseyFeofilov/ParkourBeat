using JetBrains.Annotations;
using UnityEngine;

namespace MapEditor.ChangeHandlers
{
    public abstract class ChangesHandler : MonoBehaviour
    {
        private bool _activated;

        private void LateUpdate()
        {
            if(!MainSelect.SelectedObj || IsChangeable() == null) return;
            
            if (Input.GetMouseButton(0))
            {
                if (!_activated)
                {
                    OnBeginChange();
                }
                else OnChange();
                
                _activated = true;
            }
            else if (_activated)
            {
                OnEndChange();
                _activated = false;
            }
        }
        
        private void OnMouseDown()
        {
            if(!MainSelect.SelectedObj || IsChangeable() == null) return;
            
            _activated = true;
            OnBeginChange();
        }

        private void OnMouseUp()
        {
            if(!_activated) return;
            
            _activated = false;
            OnEndChange();
        }

        /*private void Update()
        {
            if (_activated)
            {
                OnChange();
            }
        }*/

        protected abstract void OnBeginChange();
        protected abstract void OnChange();
        protected abstract void OnEndChange();
        [CanBeNull] protected abstract IChangeable IsChangeable();
    }
}