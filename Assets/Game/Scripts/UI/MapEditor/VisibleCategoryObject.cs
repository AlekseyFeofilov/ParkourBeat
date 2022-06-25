using UnityEngine;

namespace Game.Scripts.UI.MapEditor
{
    public class VisibleCategoryObject : MonoBehaviour
    {
        public GameObject category1;
        public GameObject category2;

        public void ShowCategory()
        {
            category1.SetActive(true);
            category2.SetActive(false);
        }
    }
}