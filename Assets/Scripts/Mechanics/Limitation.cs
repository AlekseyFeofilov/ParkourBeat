using UnityEngine;

namespace Mechanics
{
    public class Limitation : MonoBehaviour
    {
        private GamaManager _gamaManager;

        private void Start()
        {
            _gamaManager = FindObjectOfType<GamaManager>();
        }

        private void OnCollisionEnter()
        {
            Physics.gravity = new Vector3(0, -20f, 0);
            _gamaManager.EndGame();
        }
    }
}
