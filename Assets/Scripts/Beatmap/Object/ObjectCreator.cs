using UnityEngine;
using VisualEffect.Object;

namespace Beatmap.Object
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