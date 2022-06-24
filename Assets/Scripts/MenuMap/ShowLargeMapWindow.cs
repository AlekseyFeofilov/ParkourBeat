using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MenuMap
{
    public class ShowLargeMapWindow : MonoBehaviour
    {
        [SerializeField] GameObject _targetObj;
        private WindowInteractingWithMap _actionWindowInteractingWithMap;

        private void Start()
        {
            _actionWindowInteractingWithMap = _targetObj.GetComponent<WindowInteractingWithMap>();
        }

        public void Show(BeatmapInfo info, GameObject prefab, Transform placeForPrefabMap, GameObject currentMap)
        {
            if (placeForPrefabMap.transform.childCount > 0)
            {
                Destroy(placeForPrefabMap.transform.GetChild(0).GameObject());
            }

            var item = Instantiate(prefab, placeForPrefabMap);
            var rectTransform = prefab.GetComponent<RectTransform>();
            var position = placeForPrefabMap.position;

            rectTransform.sizeDelta = new Vector2(position.x, position.y);

            if (info.Meta.yourMap)
            {
                //показали кнопку редактирования в UI
                item.transform.GetChild(8).GameObject().SetActive(true);

                //TODO: оставила далее прослушку Роме
            }

            if (info.Meta.defaultMap)
            {
                item.transform.GetChild(9).GameObject().SetActive(true);

                //удаляем карту по клику
                Button deleteMap = item.transform.GetChild(9).GetComponent<Button>();
                deleteMap.onClick.AddListener(() =>
                {
                    _actionWindowInteractingWithMap.Delete(info.Path, currentMap, item);
                });
            }

            //вешаем запуск игры на клик
            Button startGame = item.transform.GetChild(7).GetComponent<Button>();
            startGame.onClick.AddListener(() => { _actionWindowInteractingWithMap.StartGame(); });
        }
    }
}