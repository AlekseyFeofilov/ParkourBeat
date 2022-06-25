using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.BpmMenu
{
    public class Bpm : MonoBehaviour
    {
        public TextMeshProUGUI TextBPM;

        private double _sum = 0;
        private int _countClick = 0;
        private double _lastTime = 0.0;
        
        public void MeasureBPM()
        {
            _countClick++;
            
            double time = Time.realtimeSinceStartup - _lastTime; //в секундах
            _sum += time;
            
            if (Time.realtimeSinceStartup - _lastTime > 3f)
            {
                ChangeValue("0");
                _countClick = 0;
                _sum = 0;
            } 
            else if (_countClick > 3)
            {
                int newBPM = System.Convert.ToInt32(System.Math.Floor(60 / (_sum / _countClick)));
            
                ChangeValue(newBPM.ToString());
            }
            else
            {
                int newBPM = System.Convert.ToInt32(System.Math.Floor(60 / time));

                ChangeValue(newBPM.ToString());
            }
            
            _lastTime = Time.realtimeSinceStartup;
        }

        private void ChangeValue (string valueBpm)
        {
            TextBPM.text = valueBpm.ToString();
        }
    }
}