using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Scenes
{
    public class AdvancedSceneManager : MonoBehaviour
    {
        private static AdvancedSceneManager _instance;

        public static SceneState State { get; private set; } = SceneState.Unloaded;
        public static bool Active => State == SceneState.Active;

        [SerializeField] private UnityEvent<SceneLoadEvent> onLoad;
        [SerializeField] private UnityEvent<SceneUnloadEvent> onUnload;

        private void Awake()
        {
            _instance = this;
            StartCoroutine(LoadScene());
        }
        
        private IEnumerator LoadScene()
        {
            State = SceneState.Loading;
            SceneLoadEvent @event = new();
            onLoad.Invoke(@event);
            
            yield return @event.Coroutines.GetEnumerator();
            State = SceneState.Active;
        }


        private IEnumerator UnloadScene()
        {
            State = SceneState.Unloading;
            SceneUnloadEvent @event = new();
            onUnload.Invoke(@event);
            
            yield return @event.Coroutines.GetEnumerator();
            State = SceneState.Unloaded;
        }

        private IEnumerator LoadNextScene(int buildIndex)
        {
            yield return UnloadScene();
            SceneManager.LoadScene(buildIndex);
        }
        
        private IEnumerator LoadNextScene(string sceneName)
        {
            yield return UnloadScene();
            SceneManager.LoadScene(sceneName);
        }

        public static void LoadScene(string name)
        {
            if (!_instance) 
                throw new InvalidOperationException("Script AdvancedSceneManager not found in current scene");
            if (!Active) return;
            _instance.StartCoroutine(_instance.LoadNextScene(name));
        }
        
        public static void LoadScene(int buildIndex)
        {
            if (!_instance) 
                throw new InvalidOperationException("Script AdvancedSceneManager not found in current scene");
            if (!Active) return;
            _instance.StartCoroutine(_instance.LoadNextScene(buildIndex));
        }
    }

    public class SceneUnloadEvent
    {
        public readonly ICollection<Coroutine> Coroutines = new List<Coroutine>();
    }
    
    public class SceneLoadEvent
    {
        public readonly ICollection<Coroutine> Coroutines = new List<Coroutine>();
    }

    public enum SceneState
    {
        Loading,
        Active,
        Unloading,
        Unloaded
    }
}