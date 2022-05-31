using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Bpm : MonoBehaviour
    {
        public TextMeshProUGUI TextBPM;
        //private Button ButtonBPM;
        private double Sum = 0;
        private int CountClick = 0;
        private double LastTime = 0.0;
        
        public void MeasureBPM()
        {
            //ButtonBPM = GameObject.Find("btnBPM").GetComponent<Button>();
            CountClick++;
            
            double time = Time.realtimeSinceStartup - LastTime; //в секундах
            Sum += time;
            
            if (Time.realtimeSinceStartup - LastTime > 3f)
            {
                ChangeValue("0");
                CountClick = 0;
                Sum = 0;
            } 
            else if (CountClick > 3)
            {
                int newBPM = System.Convert.ToInt32(System.Math.Floor(60 / (Sum / CountClick)));
            
                ChangeValue(newBPM.ToString());
            }
            else
            {
                int newBPM = System.Convert.ToInt32(System.Math.Floor(60 / time));

                ChangeValue(newBPM.ToString());
            }
            
            LastTime = Time.realtimeSinceStartup;
        }

        private void ChangeValue (string valueBpm)
        {
            TextBPM.text = valueBpm.ToString();
        }
    }
}