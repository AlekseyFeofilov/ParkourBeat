using Game.Scripts.Map;
using Game.Scripts.Map.Meta;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.LevelMenu
{
    /// <summary>
    /// Окно с информацией о карте
    /// </summary>
    public class ShowLargeMapWindow : MonoBehaviour
    {
        [SerializeField] GameObject _targetObj;
        private WindowInteractingWithMap _actionWindowInteractingWithMap;

        private void Start()
        {
            _actionWindowInteractingWithMap = _targetObj.GetComponent<WindowInteractingWithMap>();
        }

        public void Show(
            BeatmapInfo info,
            GameObject prefab,
            Transform placeForPrefabMap,
            GameObject currentMap,
            BeatmapManager beatmapManager)
        {
            if (placeForPrefabMap.transform.childCount > 0)
            {
                Destroy(placeForPrefabMap.transform.GetChild(0).GameObject());
            }

            var item = Instantiate(prefab, placeForPrefabMap);
            var rectTransform = prefab.GetComponent<RectTransform>();
            var position = placeForPrefabMap.position;

            rectTransform.sizeDelta = new Vector2(position.x, position.y);

            item.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = info.Meta.displayName;
            item.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = info.Meta.author;

            if (info.Meta.yoursMap)
            {
                //показали кнопку редактирования в UI
                item.transform.GetChild(8).GameObject().SetActive(true);
            }

            //прослушка редактирования игры
            Button editBtn = item.transform.GetChild(8).gameObject.GetComponent<Button>();
            editBtn.onClick.AddListener(() =>
            {
                beatmapManager.SetName(info.Name);
                beatmapManager.LoadBeatmapInEditmode();
            });

            if (info.Meta.defaultMap)
            {
                item.transform.GetChild(9).GameObject().SetActive(true);
            }

            //прослушка удаления карты
            Button deleteMap = item.transform.GetChild(9).GetComponent<Button>();
            deleteMap.onClick.AddListener(() =>
            {
                _actionWindowInteractingWithMap.Delete(info.Path, currentMap, item);
            });

            //прослушка запуска карты
            Button startBtn = item.transform.GetChild(7).gameObject.GetComponent<Button>();
            startBtn.onClick.AddListener(() =>
            {
                beatmapManager.SetName(info.Name);
                beatmapManager.LoadBeatmapInPlaymode();
            });
        }
    }
}