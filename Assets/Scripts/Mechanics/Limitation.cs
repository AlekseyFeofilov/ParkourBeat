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
            _gamaManager.EndGame();
        }
    }
}
