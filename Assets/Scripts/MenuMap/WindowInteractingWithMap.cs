using System.IO;
using UnityEngine;

namespace MenuMap
{
    public class WindowInteractingWithMap : MonoBehaviour
    {
        public void AddNewMap()
        {
            //TODO: оставила прослушку Роме
        }

        public void Delete(string path)
        {
            //удаляем карту - папку из списка
            RecursiveDirectoryDelete(path);

            //удаляем карту из листа UI
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