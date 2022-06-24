using UnityEngine;

namespace Mechanics
{
    public class Limitation : MonoBehaviour
    {
        private GameManager _gamaManager;

        private void Start()
        {
            _gamaManager = FindObjectOfType<GameManager>();
        }

        private void OnCollisionEnter()
        {
            _gamaManager.EndGame(0);
        }
    }
}
