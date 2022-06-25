using Game.Scripts.Map.VisualEffect.Object;
using UnityEngine;

namespace Game.Scripts.Map.Manager.Creator
{
    public class ObjectCreator : MonoBehaviour
    {
        [SerializeField] private ObjectManager objectManager;
        
        public void CreateCubeFromSelectedObject()
        {
            if (Camera.main is null) return;

            Vector3 pos = Camera.main.transform.position;
            CubeObject obj = (CubeObject) objectManager.CreateObject("cube");
            obj.Position.SetDefault(pos + 3 * Camera.main.transform.forward);
        }
    }
}