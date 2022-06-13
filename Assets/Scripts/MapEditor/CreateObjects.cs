using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    public class CreateObjects : MonoBehaviour
    {
        public Camera Camera;
        public GameObject SettingsMenu;
        [SerializeField] private Transform _contentContainerListObject;
        [SerializeField] private GameObject _settingsPrefab;
        private List<ObjectMap> _objects = new List<ObjectMap>();
        GameObject TargetObj;
        private Object _actionTarget;

        public void CreateCube()
        {
            //метод добавления куба, но может можно сделать по-другому
            GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObject.transform.position = Camera.transform.position + (3 * Camera.transform.forward);
            newObject.transform.Rotate(300, 0, 200);

            ObjectMap newObj = new ObjectMap(newObject, _settingsPrefab);

            ShowSettingsObject();
            _objects.Add(newObj);
        }

        public void CreateDecorationFog()
        {
            //TODO:как-нибудь сделать в будущем
            //не совсем понимаю, каким он должен быть в игре
        }


        //показать меню настроек, как только пользователь нажмет на нужный объект
        //в спике объектов или в редакторе
        private void ShowSettingsObject()
        {
            if (SettingsMenu.transform.childCount > 2)
            {
                SettingsMenu.transform.GetChild(1);
            }

            GameObject settingsObj = (GameObject)Instantiate(_settingsPrefab);
            settingsObj.transform.SetParent(SettingsMenu.transform, false);

            SettingsMenu.SetActive(true);
        }

        //изменение настроек объекта
        // public void UpdateSettingsObject(ObjectMap obj)
        // {
        //     var _contentContainerSettings = _settingsPrefab.GetComponentInChildren<Transform>();
        //     
        //     for (int i = 0; i < _contentContainerSettings.childCount; i++)
        //     {
        //         var child = _contentContainerSettings.GetChild(i);
        //         
        //         
        //     }
        //     
        // }

        //добавление объекта в список объектов
        //то есть просто добавляем текст в Scroll View
        // private void AddObjectToList()
        // {
        //     var item = Instantiate(_settingsPrefab);
        //
        //     item.GetComponentInChildren<Text>().text = "title";
        //     item.transform.SetParent(_contentContainerListObject);
        //     item.transform.localScale = Vector2.one;
        //
        //     // ScrollView listObj = GameObject.Find("Scroll List Object").GetComponent<ScrollView>();
        //     // _title.text = "Bla";
        //     //
        //     // listObj.contentContainer.Children(_title);
        // }
    }
}