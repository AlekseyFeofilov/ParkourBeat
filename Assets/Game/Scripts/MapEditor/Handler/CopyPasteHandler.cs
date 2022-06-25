using Game.Scripts.Engine.Manager;
using Game.Scripts.Engine.Select;
using Game.Scripts.Map.Manager;
using Game.Scripts.Map.VisualEffect.Object;
using Libraries.QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.MapEditor.Handler
{
    public class CopyPasteHandler : MonoBehaviour
    {
        [SerializeField] private ObjectManager objectManager;
        [FormerlySerializedAs("mainSelect")] [SerializeField] private SelectManager selectManager;

        public void OnPaste(Buffer buffer)
        {
            selectManager.Deselect();
            foreach (var (gameObject, position) in buffer.Objects)
            {
                if (gameObject.TryGetComponent(out CubeObject cubeObject))
                {
                    CubeObject copy = (CubeObject) objectManager.CopyObject(cubeObject);
                    copy.Position.Default = Camera.main.transform.position + position - buffer.Center;
                    if (copy.TryGetComponent(out OutlinedObject outlinedObject))
                    {
                        selectManager.Select(outlinedObject);
                    }
                }
            }
        }
    }
}