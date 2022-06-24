using Beatmap.Object;
using MapEditor.Select;
using UnityEngine;
using VisualEffect.Object;

namespace MapEditor
{
    public class CopyPasteHandler : MonoBehaviour
    {
        [SerializeField] private ObjectManager objectManager;
        [SerializeField] private MainSelect mainSelect;

        public void OnPaste(Buffer buffer)
        {
            mainSelect.Deselect();
            foreach (var (gameObject, position) in buffer.Objects)
            {
                if (gameObject.TryGetComponent(out CubeObject cubeObject))
                {
                    CubeObject copy = (CubeObject) objectManager.CopyObject(cubeObject);
                    copy.Position.Default = Camera.main.transform.position + position - buffer.Center;
                    if (copy.TryGetComponent(out OutlinedObject outlinedObject))
                    {
                        mainSelect.Select(outlinedObject);
                    }
                }
            }
        }
    }
}