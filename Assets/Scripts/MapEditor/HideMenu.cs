using UnityEngine;

public class HideMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject button;

    public void MenuHide()
    {
        menu.SetActive(false);
        button.SetActive(true);
    }
}