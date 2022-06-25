using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace Game.Scripts.Setup
{
    public class DefaultBeatmapInstaller : MonoBehaviour
    {
        private void Start()
        {
            string folder = $"{Application.persistentDataPath}/Songs";

            if (Directory.Exists(folder)) return;
            
            var songs = Resources.Load<TextAsset>("Songs");
            string zip = $"{folder}.zip";
            File.WriteAllBytes(zip, songs.bytes);
                
            ZipFile.ExtractToDirectory(zip, Application.persistentDataPath);
        }
    }
}