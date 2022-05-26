using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class AudioController : MonoBehaviour
{
    public AudioType audioType = AudioType.MPEG;
    
    public string path = "Songs/korpvodk.mp3";
    
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(LoadAudio());
    }

    private IEnumerator LoadAudio()
    {
        string fullPath = "file://" + Application.dataPath + "/" + path;
        
        using UnityWebRequest request 
            = UnityWebRequestMultimedia.GetAudioClip(fullPath, audioType);
        yield return request.SendWebRequest();

        if (request.result is 
            UnityWebRequest.Result.ConnectionError or 
            UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            audioSource.clip = DownloadHandlerAudioClip.GetContent(request);
            audioSource.clip.name = Regex.Match(path, @"[^\/]*$").Value;
            audioSource.Play();
        }
    }
}