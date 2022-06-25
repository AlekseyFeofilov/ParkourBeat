using Game.Scripts.Engine.Manager;
using Libraries.QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Map.Manager.Selector
{
    public class ObjectSelector : MonoBehaviour
    {
        [SerializeField] private ObjectManager objectManager;
        [FormerlySerializedAs("mainSelect")] [SerializeField] private SelectManager selectManager;

        public void SelectSky()
        {
            var outlined = objectManager.skyObject.GetComponent<OutlinedObject>();
            selectManager.Deselect();
            selectManager.Select(outlined);
        }

        public void SelectHorizon()
        {
            var outlined = objectManager.horizonObject.GetComponent<OutlinedObject>();
            selectManager.Deselect();
            selectManager.Select(outlined);
        }
    }
}