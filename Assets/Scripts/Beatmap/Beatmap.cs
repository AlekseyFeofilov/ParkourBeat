﻿using System.IO;
using Beatmap.Object;
using MapEditor;
using MapEditor.Timestamp;
using Newtonsoft.Json;
using Serialization;
using Serialization.Converter;
using Serialization.Data;
using UnityEngine;
using Utils;

namespace Beatmap
{
    public abstract class Beatmap : MonoBehaviour
    {
        [SerializeField] protected Camera camera;
        
        [SerializeField] public Timeline timeline;
        [SerializeField] public ObjectManager objectManager;
        [SerializeField] public AudioSource songSource;
        [SerializeField] public string beatmapName;
        
        protected string Folder => $"{Application.persistentDataPath}/Songs/{beatmapName}"; 
        protected string FileBeatmap => $"{Folder}/beatmap.json";
        protected string FileSong => $"{Folder}/song.mp3";
        
        protected readonly JsonManager JsonManager = new();

        public abstract void PlayAudio();
        public abstract void StopAudio();

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
            StartCoroutine(AudioUtils.LoadAudio(FileSong, AudioType.MPEG, songSource));
        }
        
        public virtual void Load()
        {
            if (!Directory.Exists(Folder))
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
        }
    }
}