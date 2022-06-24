using MapEditor.Select;
using UnityEngine;

namespace Beatmap.Object
{
    public class ObjectSelector : MonoBehaviour
    {
        [SerializeField] private ObjectManager objectManager;
        [SerializeField] private MainSelect mainSelect;

        public void SelectSky()
        {
            var outlined = objectManager.skyObject.GetComponent<OutlinedObject>();
            mainSelect.Deselect();
            mainSelect.Select(outlined);
        }

        public void SelectHorizon()
        {
            var outlined = objectManager.horizonObject.GetComponent<OutlinedObject>();
            mainSelect.Deselect();
            mainSelect.Select(outlined);
        }
    }
}