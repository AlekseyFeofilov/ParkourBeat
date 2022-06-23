using System.Collections.Generic;
using System.IO;
using Serialization;
using TMPro;
using UnityEngine;

namespace MenuMap
{
    public class BeatmapListManager : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private GameObject prefab;
        private string _folder;
        private readonly JsonManager _jsonManager = new JsonManager();

        void Start()
        {
            _folder = $"{Application.persistentDataPath}/Songs";

            foreach (var beatmapInfo in GetListBeatmap())
            {
                var item = Instantiate(prefab, container);
                var rectTransform = prefab.GetComponent<RectTransform>();

                rectTransform.sizeDelta = new Vector2(116, container.position.y);

                //заполнить данные

                item.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "bla";
                item.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = beatmapInfo.Meta.author;

                // Instantiate(item);
                //item.transform.SetParent(container); 
                //item.transform.localScale = Vector2.one;

                // var targetTransform = container.transform;
                // Instantiate(prefab, Vector3.zero, Quaternion.identity, targetTransform);
            }
        }

        private List<BeatmapInfo> GetListBeatmap()
        {
            List<BeatmapInfo> _listMaps = new();

            foreach (var beatmapDir in Directory.GetDirectories(_folder))
            {
                string metaFile = $"{beatmapDir}/meta.json";
                if (File.Exists(metaFile))
                {
                    //подгружаем данные с папки с картами
                    string jsonString = File.ReadAllText(metaFile);
                    BeatmapMeta metaData = _jsonManager.Deserialize<BeatmapMeta>(jsonString);

                    string name = Path.GetDirectoryName(beatmapDir);
                    BeatmapInfo metaInfo = new BeatmapInfo(name, metaData);

                    _listMaps.Add(metaInfo);
                }
            }

            return _listMaps;
        }
    }
}