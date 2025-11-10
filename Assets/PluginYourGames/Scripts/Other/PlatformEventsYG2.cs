using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YG.Insides
{
    public class PlatformEventsYG2 : MonoBehaviour
    {
        public List<string> platforms = new List<string>();
        public UnityEvent platformAction;
        public enum UpdateType
        {
            Awake,
            Start,
            OnEnable,
            OnDisable,
#if RU_YG2
            [InspectorName("Вручную (метод ExecuteEvent)")]
#else
            [InspectorName("Manual (method ExecuteEvent)")]
#endif
            Manual
        }
        public UpdateType whenToEvent = UpdateType.OnEnable;

        public enum ExecuteMode
        {
#if RU_YG2
            [InspectorName("Выполнять только у выбранных платформ")]
#else
            [InspectorName("Run only on selected platforms")]
#endif
            Selected,
#if RU_YG2
            [InspectorName("Игнорировать выбранные платформы (выполнять у тех, которых нет в списке)")]
#else
            [InspectorName("Ignore selected platforms (perform on those that are not in the list)")]
#endif
            Deselected
        }
        public ExecuteMode executeMode = ExecuteMode.Selected;

        private void Awake()
        {
            if (whenToEvent == UpdateType.Awake)
                ExecuteEvent();
        }

        private void Start()
        {
            if (whenToEvent == UpdateType.Start)
                ExecuteEvent();
        }

        private void OnEnable()
        {
            if (whenToEvent == UpdateType.OnEnable)
                ExecuteEvent();
        }

        private void OnDisable()
        {
            if (whenToEvent == UpdateType.OnDisable)
                ExecuteEvent();
        }

        public void ExecuteEvent()
        {
            bool isContainsCurrentPlatform = platforms.Contains(YG2.platform);

            if (executeMode == ExecuteMode.Selected)
            {
                if (isContainsCurrentPlatform)
                {
                    platformAction?.Invoke();
                }
            }
            else if (executeMode == ExecuteMode.Deselected)
            {
                if (!isContainsCurrentPlatform)
                {
                    platformAction?.Invoke();
                }
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (platformAction == null)
                platformAction = new UnityEvent();

            UnityEditor.Events.UnityEventTools.AddPersistentListener(platformAction, DeactivateGameObject);
        }
#endif

        public void DeactivateGameObject()
        {
            gameObject.SetActive(false);
        }
    }
}