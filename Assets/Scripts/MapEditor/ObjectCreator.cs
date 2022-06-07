using UnityEngine;

namespace MapEditor
{
    public class ObjectCreator : MonoBehaviour
    {
        [SerializeField]
        private Camera player;

        [SerializeField]
        private GameObject prefab;
        
        public void Place()
        {
            GameObject obj 
                = Instantiate(prefab, player.transform.position, Quaternion.identity);
        }
    }
}