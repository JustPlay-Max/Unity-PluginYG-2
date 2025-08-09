using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YG.MenuNav
{
    public class PointerEnterHelperNav : MonoBehaviour
    {
        private EventTrigger eventTrigger;
        private Button buttonSelect;
        private CursorSelectButton cursorSelectScr;

        public void Init(Button button, CursorSelectButton cursorSelectButton)
        {
            buttonSelect = button;
            cursorSelectScr = cursorSelectButton;

            if (!eventTrigger)
                eventTrigger = button?.GetComponent<EventTrigger>();

            if (!eventTrigger)
                eventTrigger = button.gameObject.AddComponent<EventTrigger>();

            if (!HasEventTriggerEntry(eventTrigger, EventTriggerType.PointerEnter, OnPointerEnter))
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
                eventTrigger.triggers.Add(entry);
            }

            if (!HasEventTriggerEntry(eventTrigger, EventTriggerType.PointerClick, OnPointerClick))
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
                eventTrigger.triggers.Add(entry);
            }

            if (!HasEventTriggerEntry(eventTrigger, EventTriggerType.PointerExit, OnPointerExit))
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
                eventTrigger.triggers.Add(entry);
            }
        }

        private void OnPointerEnter(BaseEventData data)
        {
            if (buttonSelect && buttonSelect.enabled && buttonSelect.interactable)
            {
                MenuNavigation.Instance.SelectButton(buttonSelect);
                cursorSelectScr.PointerEnterCallback();
            }
        }

        private void OnPointerClick(BaseEventData data)
        {
            if (buttonSelect && buttonSelect.enabled)
                cursorSelectScr.PointerClickCallback(buttonSelect);
        }

        private void OnPointerExit(BaseEventData data)
        {
            MenuNavigation.Instance.UnselectButton(true);
            cursorSelectScr.PointerExitCallback();
        }

        private bool HasEventTriggerEntry(EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> action)
        {
            foreach (EventTrigger.Entry entry in trigger.triggers)
            {
                if (entry.eventID == eventType &&
                    (entry.callback.GetPersistentEventCount() == 0 ||
                    entry.callback.GetPersistentMethodName(0) == action.Method.Name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}