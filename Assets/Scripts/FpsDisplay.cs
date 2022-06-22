using TMPro;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    private const string Display = "{0} FPS";
        
    private float _timer;
    private float _refresh;
    private float _averageFramerate;

    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        _timer = _timer <= 0 ? _refresh : _timer -= timelapse;

        if (_timer <= 0) _averageFramerate = (int) (1f / timelapse);
    }

    private void FixedUpdate()
    {
        text.text = string.Format(Display, _averageFramerate);
    }
}