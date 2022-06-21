using MapEditor.Select;
using UnityEngine;

namespace MapEditor
{
    public class SampleSelectableObject : MonoBehaviour, ISelectable
    {
        public void OnSelect(SelectEvent @event)
        {
            @event.Cancelled = false;
        }

        public void OnDeselect(DeselectEvent @event)
        {
            @event.Cancelled = false;
        }
    }
}
