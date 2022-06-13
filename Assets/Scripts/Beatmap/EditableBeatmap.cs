using System;
using System.IO;
using MapEditor;
using MapEditor.Trigger;
using Serialization.Data;
using UnityEngine;

namespace Beatmap
{
    public class EditableBeatmap : Beatmap, IStored
    {
        [SerializeField] public TriggerManager triggerManager;
        
        [SerializeField] private AudioSource soundSource;
        [SerializeField] private GameObject wallPrefab;
        
        private GameObject _wall;
        private int _lastBeat = -1;
        
        private string FileEditor => $"{Folder}\\editor.json";

        private void Update()
        {
            double time = songSource.isPlaying 
                // Если музыка играет, то обновляем эффекты по времени музыки
                ? songSource.time 
                // Иначе обновляем эффекты по расположению игрока
                : timeline.GetSecondByPosition(camera.transform.position.x);

            // Обновляем эффекты
            timeline.Move(time);

            // Далее только для режима с воспроизведением музыки
            if (!songSource.isPlaying) return;
            
            // Движем стену
            double x = timeline.GetPositionBySecond(time);
            Vector3 position = _wall.transform.position;
            _wall.transform.position = new Vector3((float) x, position.y, position.z);

            // Биты
            int beat = (int) Math.Floor(timeline.GetBeatBySecond(time));
            if (_lastBeat == beat) return;
            _lastBeat = beat;
            soundSource.Play();
        }

        /// <summary>
        /// Синхронизирует время музыки с расположением игрока
        /// </summary>
        public void SynchronizeAudioWithPosition()
        {
            double time = timeline.GetSecondByPosition(camera.transform.position.x);
            int beat = (int) Math.Floor(timeline.GetBeatBySecond(time));

            if (beat >= 0)
            {
                time = timeline.GetSecondByBeat(beat);
            }

            songSource.time = Mathf.Max(0, (float) time);
        }

        /// <summary>
        /// Обнуляет время музыки до нуля
        /// </summary>
        public void ResetAudioTimestamp()
        {
            songSource.time = 0;
        }
        
        public void PlayAudio()
        {
            StopAudio();
            Vector3 scale = wallPrefab.transform.localScale;
            
            _wall = Instantiate(wallPrefab, new Vector3(0, scale.y / 2, 0), Quaternion.identity);
            _wall.transform.parent = transform;

            _lastBeat = -1;
            songSource.Play();
        }

        public void StopAudio()
        {
            if (_wall == null) return;
            Destroy(_wall);
            _wall = null;
            
            songSource.Stop();
        }
        
        public void Save()
        {
            if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);
            
            BeatmapData data = new();
            SaveData(data);
            string json = JsonManager.Serialize(data);
            File.WriteAllText(FileBeatmap, json);
        }

        public void SaveData(BeatmapData data)
        {
            objectManager.SaveData(data);
            timeline.SaveData(data);
        }
    }
}