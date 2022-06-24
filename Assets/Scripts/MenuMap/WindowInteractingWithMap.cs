using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace MenuMap
{
    public class WindowInteractingWithMap : MonoBehaviour
    {
        public void AddNewMap()
        {
            //TODO: оставила прослушку Роме
        }

        public void Delete(string path, GameObject map, GameObject manyInfoMap)
        {
            //удаляем карту - папку из списка
            RecursiveDirectoryDelete(path);

            //удаляем карту из листа UI
            Destroy(map.GameObject());
            Destroy(manyInfoMap.GameObject());
        }

        public void StartGame()
        {
            //TODO: оставила прослушку Роме
        }

        private void RecursiveDirectoryDelete(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                RecursiveDirectoryDelete(dir);
            }

            Directory.Delete(path);
        }
    }
}