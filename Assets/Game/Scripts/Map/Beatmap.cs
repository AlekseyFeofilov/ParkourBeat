using System.Collections;
using System.IO;
using Game.Scripts.Map.Manager;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Serialization;
using Game.Scripts.Serialization.Converter;
using Game.Scripts.Serialization.Data;
using Game.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Scripts.Map
{
    public abstract class Beatmap : MonoBehaviour
    {
        [SerializeField] protected Camera camera;
        [SerializeField] public Timeline.Timeline timeline;
        [SerializeField] public ObjectManager objectManager;
        [SerializeField] public AudioSource songSource;
        [SerializeField] public string beatmapName;

        public double AudioTime =>
            songSource.clip is null ? 0 : (double)songSource.timeSamples / songSource.clip.frequency;

        // Я ОТОШЕЛ НА СЕК
        
        protected string Folder => $"{Application.persistentDataPath}/Songs/{beatmapName}";
        protected string FileBeatmap => $"{Folder}/beatmap.json";
        protected string FileSong => $"{Folder}/song.mp3";

        protected readonly JsonManager JsonManager = new();

        protected double End;
        protected float Fog;

        public abstract void PlayAudio();
        public abstract void StopAudio();

        protected virtual void OnInitialized()
        {
        }

        protected virtual void Start()
        {
            if (camera == null) camera = Camera.main;
            if (string.IsNullOrWhiteSpace(beatmapName)) beatmapName = BeatmapManager.CurrentName;

            JsonManager.AddConverter(new Vector2Converter());
            JsonManager.AddConverter(new Vector3Converter());
            JsonManager.AddConverter(new ColorConverter());
            JsonManager.AddConverter(new ValueConverter());
            JsonManager.AddConverter(new TimeConverter());
            JsonManager.AddConverter(new TimingFunctionConverter());
            JsonManager.Formatting = Formatting.Indented;

            Load();
            StartCoroutine(LoadAudio());
        }

        private IEnumerator LoadAudio()
        {
            yield return AudioUtils.LoadAudio(FileSong, AudioType.MPEG, songSource);
            OnInitialized();
        }

        public virtual void Load()
        {
            if (!Directory.Exists(Folder) || !File.Exists(FileBeatmap))
            {
                timeline.AddBpmPoint(MapTime.Zero, 60);
                timeline.AddSpeedPoint(MapTime.Zero, 1);
                return;
            }

            string json = File.ReadAllText(FileBeatmap);
            BeatmapData data = JsonManager.Deserialize<BeatmapData>(json);
            LoadData(data);
        }

        public void LoadData(BeatmapData data)
        {
            objectManager.LoadData(data);
            timeline.LoadData(data);
            Fog = data.Settings.Fog;
            End = data.Settings.End;
        }
    }
}