using Beatmap;
using UnityEngine;

public class ShowMenuGame : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private PlayableBeatmap beatmap;
    [SerializeField] private HideCursorScript hideCursorScript;
        
    private bool _activeMenu = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_activeMenu)
        {
            ShowMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _activeMenu)
        {
            HideMenu();
        }
    }

    private void ShowMenu()
    {
        menu.SetActive(true);
        _activeMenu = true;
        if (beatmap != null)
        {
            beatmap.PauseAudio();
        }

        if (hideCursorScript != null)
        {
            hideCursorScript.Show();
        }
    }


    public void HideMenu()
    {
        menu.SetActive(false);
        _activeMenu = false;
        if (beatmap != null)
        {
            beatmap.ContinueAudio();
        }
    }
}