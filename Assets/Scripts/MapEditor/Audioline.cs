using System;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;

namespace MapEditor
{
    public class Audioline : MonoBehaviour
    {
        [SerializeField]
        private Timeline timeline;

        [SerializeField]
        private AudioSource songSource;
        
        [SerializeField]
        private AudioSource soundSource;
        
        [SerializeField]
        private GameObject wallPrefab;

        private GameObject _wall;
        private int _lastBeat = -1;

        private void Update()
        {
            if (!songSource.isPlaying) return;

            double time = songSource.time;
            double x = timeline.GetPositionByTime(time);
            Vector3 position = _wall.transform.position;

            _wall.transform.position = new Vector3((float) x, position.y, position.z);

            int beat = (int) Math.Floor(timeline.GetBeatByTime(time));

            if (_lastBeat == beat) return;
            
            _lastBeat = beat;
            foreach (var o in timeline.BeatObjects[beat])
            {
                PlacedObject animated = o.GetComponent<PlacedObject>();
                Vector3 scale = o.transform.localScale * 1;
                animated.Scale.Value = scale * 1.1f;
                animated.Scale.BeginTransition(scale, .5f, ITimingFunction.Ease);
                soundSource.Play();
            }
        }

        public void PlayAudio()
        {
            StopAudio();
            Vector3 scale = wallPrefab.transform.localScale;
            
            _wall = Instantiate(wallPrefab, new Vector3(0, scale.y / 2, 0), Quaternion.identity);
            _wall.transform.parent = transform;

            _lastBeat = -1;
            songSource.Play();
        }

        public void StopAudio()
        {
            if (_wall == null) return;
            Destroy(_wall);
            _wall = null;
            
            songSource.Stop();
        }
    }
}
