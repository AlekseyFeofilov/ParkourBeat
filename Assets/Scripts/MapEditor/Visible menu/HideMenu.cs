using JetBrains.Annotations;
using UnityEngine;

public class HideMenu : MonoBehaviour
{
    public GameObject menu;
    [CanBeNull] public GameObject button;

    public void MenuHide()
    {
        menu.SetActive(false);
        if (button != null)
        {
            button.SetActive(true);
        }
    }
}