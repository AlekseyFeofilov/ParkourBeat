using JetBrains.Annotations;
using MapEditor.Select;
using UnityEngine;

namespace MapEditor.ChangeHandlers
{
    public abstract class ChangesHandler : MonoBehaviour
    {
        private bool _activated;

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

        private void Update()
        {
            if (_activated)
            {
                OnChange();
            }
        }

        protected abstract void OnBeginChange();
        protected abstract void OnChange();
        protected abstract void OnEndChange();
        [CanBeNull] protected abstract IChangeable IsChangeable();
    }
}