using Game.Scripts.Engine.Manager;
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
            selectManager.Deselect();
            selectManager.Select(objectManager.skyObject.gameObject);
        }

        public void SelectHorizon()
        {
            selectManager.Deselect();
            selectManager.Select(objectManager.horizonObject.gameObject);
        }
    }
}