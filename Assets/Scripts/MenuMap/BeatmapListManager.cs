using System.Collections.Generic;
using System.IO;
using Beatmap;
using JetBrains.Annotations;
using Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuMap
{
    public class BeatmapListManager : MonoBehaviour
    {
        [SerializeField] private BeatmapManager beatmapManager;
        [SerializeField] private Transform container;
        [SerializeField] private GameObject prefabMap;
        [SerializeField] private GameObject prefabLargeMap;
        [SerializeField] private Transform placeForPrefabMap;
        private string _folder;
        private readonly JsonManager _jsonManager = new JsonManager();
        [SerializeField] GameObject targetObj;
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
                });
            }
        }

        //формируем лист с мета-данными каждой карты
        private List<BeatmapInfo> GetListBeatmap()
        {
            List<BeatmapInfo> _listMapsInfo = new();

            foreach (var beatmapDir in Directory.GetDirectories(_folder))
            {
                string metaFile = $"{beatmapDir}/meta.json";
                if (File.Exists(metaFile))
                {
                    //подгружаем данные с папки с картами
                    string jsonString = File.ReadAllText(metaFile);
                    BeatmapMeta metaData = _jsonManager.Deserialize<BeatmapMeta>(jsonString);

                    string name = Path.GetDirectoryName(beatmapDir);
                    BeatmapInfo metaInfo = new BeatmapInfo(name, metaData, beatmapDir);

                    _listMapsInfo.Add(metaInfo);
                }
            }

            return _listMapsInfo;
        }
    }
}


// ShowLargeMapWindow script = item.AddComponent<ShowLargeMapWindow>();
// script.Info = beatmapInfo;
// script.prefab = prefabLargeMap;
// script.placeForPrefabMap = placeForPrefabMap;