using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Engine.Select
{
    public static class UIRaycaster
    {
        public static bool PointerIsOverUI(Vector2 screenPos)
        {
            var hitObject = UIRaycast(ScreenPositionToPointerData(screenPos));
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            return hitObject != null && hitObject.layer == LayerMask.NameToLayer("UI");
        }

        private static GameObject UIRaycast(PointerEventData pointerData)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            return results.Count < 1 ? null : results[0].gameObject;
        }

        private static PointerEventData ScreenPositionToPointerData(Vector2 screenPos)
            => new(EventSystem.current) { position = screenPos };
    }
}