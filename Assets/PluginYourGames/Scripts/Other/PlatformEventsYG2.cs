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
#endif
            Manual
        }
        public UpdateType whenToEvent = UpdateType.Start;

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
            if (platforms.Contains(YG2.platform))
            {
                platformAction?.Invoke();
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