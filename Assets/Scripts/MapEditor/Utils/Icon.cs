using UnityEngine;

namespace MapEditor.Utils
{
    public class Icon : MonoBehaviour
    {
        [SerializeField]
        private Camera eyes;
    
        private void Update()
        {
            Vector3 direction = transform.position - eyes.transform.position;
            direction.Normalize();

            transform.rotation = Quaternion.LookRotation(direction);
            transform.Rotate(-90, 0, 0);
        }
    }
}
