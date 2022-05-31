using System;
using UnityEngine;

namespace DefaultNamespace.Audio
{
    public class StartMusicOnButton : MonoBehaviour
    {
        //для обращения к функциям из другого скрипта
        GameObject TargetObj;
        private AudioController _actionTarget;
        
        public void Start()
        {
            _actionTarget = TargetObj.GetComponent<AudioController>();
            MeasureBPM();
        }
        
        private void MeasureBPM()
        {
            _actionTarget.Start();
        }
        
    }
}