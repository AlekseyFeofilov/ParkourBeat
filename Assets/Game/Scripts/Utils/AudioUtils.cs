using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Scripts.Utils
{
    public class AudioUtils
    {
        public static IEnumerator LoadAudio(string path, AudioType audioType, AudioSource target)
        {
            string fullPath = "file://" + path;
        
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
                target.clip = DownloadHandlerAudioClip.GetContent(request);
                target.clip.name = Regex.Match(path, @"[^\/]*$").Value;
            }
        }
    }
}