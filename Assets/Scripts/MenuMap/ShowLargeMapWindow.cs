using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MenuMap
{
    public class ShowLargeMapWindow : MonoBehaviour
    {
        private GameObject _targetObj;
        private WindowInteractingWithMap _actionWindowInteractingWithMap;

        private void Start()
        {
            _actionWindowInteractingWithMap = _targetObj.GetComponent<WindowInteractingWithMap>();
        }

        public void Show(BeatmapInfo Info, GameObject prefab, Transform placeForPrefabMap)
        {
            var item = Instantiate(prefab, placeForPrefabMap);
            var rectTransform = prefab.GetComponent<RectTransform>();
            var position = placeForPrefabMap.position;

            rectTransform.sizeDelta = new Vector2(position.x, position.y);

            if (Info.Meta.yourMap)
            {
                //показали кнопку редактирования в UI
                item.transform.GetChild(8).GameObject().SetActive(true);

                //TODO: оставила далее прослушку Роме
            }

            if (Info.Meta.defaultMap)
            {
                item.transform.GetChild(9).GameObject().SetActive(true);

                //удаляем карту по клику
                Button deleteMap = item.transform.GetChild(9).GetComponent<Button>();
                deleteMap.onClick.AddListener(() => { _actionWindowInteractingWithMap.Delete(Info.Path); });
            }

            //вешаем запуск игры на клик
            Button startGame = item.transform.GetChild(7).GetComponent<Button>();
            startGame.onClick.AddListener(() => { _actionWindowInteractingWithMap.StartGame(); });
        }
    }
}