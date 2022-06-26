using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class VersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            text.text = string.Format(text.text, Application.version);
        }
    }
}