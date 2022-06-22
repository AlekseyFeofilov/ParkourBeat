using UnityEngine;

namespace MenuMap
{
    public class WindowInteractingWithMap : MonoBehaviour
    {
        private string _difficultyLevel; //easy, medium, hard

        public void StartGame()
        {
            //TODO: оставила прослушку Роме
        }

        public void EditMap()
        {
            //TODO: оставила прослушку Роме
        }

        public void DeleteMap()
        {
            //TODO: СДЕЛАТЬ УДАЛЕНИЕ ИЗ СПИСКА И ПАПКУ КАРТЫ
        }

        public void ChangeDifficultyLevel(string difficultyLevel)
        {
            _difficultyLevel = difficultyLevel;
        }
    }
}