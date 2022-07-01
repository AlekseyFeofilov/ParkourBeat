using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game.Scripts.Serialization;
using Game.Scripts.UI.LevelMenu;
using Game.Scripts.Utils;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AudioSettings = Game.Scripts.Settings.AudioSettings.AudioSettings;

namespace Game.Scripts.Map.Meta
{
    public class BeatmapListManager : MonoBehaviour
    {
        [SerializeField] private BeatmapManager beatmapManager;
        [SerializeField] private Transform container;
        [SerializeField] private GameObject prefabMap;
        [SerializeField] private GameObject prefabLargeMap;
        [SerializeField] private Transform placeForPrefabMap;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] GameObject targetObj;

        private Coroutine _coroutine;
        private string _folder;
        private readonly JsonManager _jsonManager = new JsonManager();
        [CanBeNull] private ShowLargeMapWindow _actionTarget;

        void Start()
        {
            _folder = $"{Application.persistentDataPath}/Songs";
            _actionTarget = targetObj.GetComponent<ShowLargeMapWindow>();

            foreach (var beatmapInfo in GetListBeatmap())
            {
                var item = Instantiate(prefabMap, container);
                var rectTransform = prefabMap.GetComponent<RectTransform>();

                rectTransform.sizeDelta = new Vector2(116, container.position.y);

                //заполняем данные
                item.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    beatmapInfo.Meta.displayName;
                item.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = beatmapInfo.Meta.author;
                var bla1 = beatmapInfo.Meta.defaultMap;


                //вешаем показать подробную инфу о карте по клику
                Button button = item.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    _actionTarget.Show(beatmapInfo, prefabLargeMap, placeForPrefabMap, item, beatmapManager);
                    PlaySong(beatmapInfo);
                });
            }
        }

        //формируем лист с мета-данными каждой карты
        private List<BeatmapInfo> GetListBeatmap()
        {
            List<BeatmapInfo> _listMapsInfo = new();

            foreach (var dir in Directory.GetDirectories(_folder))
            {
                string beatmapDir = dir.Replace("\\", "/");
                string metaFile = $"{beatmapDir}/meta.json";
                if (File.Exists(metaFile))
                {
                    //подгружаем данные с папки с картами
                    string jsonString = File.ReadAllText(metaFile);
                    BeatmapMeta metaData = _jsonManager.Deserialize<BeatmapMeta>(jsonString);

                    string name = new DirectoryInfo(beatmapDir).Name;
                    BeatmapInfo metaInfo = new BeatmapInfo(name, metaData, beatmapDir);

                    _listMapsInfo.Add(metaInfo);
                }
            }

            return _listMapsInfo;
        }

        private void PlaySong(BeatmapInfo info)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(LoadAudioAndPlay($"{info.Path}/song.mp3", info.Meta.menuSongTimestamp));
        }
        
        private IEnumerator LoadAudioAndPlay(string fileSong, float songTimestamp)
        {
            if (audioSource.isPlaying)
            {
                for (int i = 20; i >= 0; i--)
                {
                    audioSource.volume = i / 20f * AudioSettings.MusicVolume;
                    yield return new WaitForSeconds(0.01f);
                }
            }
            audioSource.Stop();
            yield return AudioUtils.LoadAudio(fileSong, AudioType.MPEG, audioSource);
            audioSource.time = songTimestamp;
            audioSource.Play();
            for (int i = 0; i <= 20; i++)
            {
                audioSource.volume = i / 20f * AudioSettings.MusicVolume;
                yield return new WaitForSeconds(0.01f);
            }
            _coroutine = null;
        }
    }
}


// ShowLargeMapWindow script = item.AddComponent<ShowLargeMapWindow>();
// script.Info = beatmapInfo;
// script.prefab = prefabLargeMap;
// script.placeForPrefabMap = placeForPrefabMap;