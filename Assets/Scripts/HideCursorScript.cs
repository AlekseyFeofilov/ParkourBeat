using UnityEngine;

public class HideCursorScript : MonoBehaviour
{
    private void OnEnable()
    {
        Hide();
    }

    private void OnDisable()
    {
        Show();
    }

    public void Hide()
    {
        Cursor.visible = false;
    }

    public void Show()
    {
        Cursor.visible = true;
    }
}