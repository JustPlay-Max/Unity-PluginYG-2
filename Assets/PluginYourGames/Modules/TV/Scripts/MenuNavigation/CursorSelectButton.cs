using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace YG.MenuNav
{
    public class CursorSelectButton : MonoBehaviour
    {
        public UnityEvent PointEnterEvent;
        public UnityEvent PointClickEvent;
        public UnityEvent PointExitEvent;
        public UnityEvent PointClickEnterEvent;
        public UnityEvent PointClickNoEnterEvent;

        private bool active;
        public static Action onPointEnter, onPointClick, onPointClickEnter, onPointClickNoEnter, onPointExit;

        private void OnEnable()
        {
            MenuNavigation.onIsActiveMavigation += Setup;
            MenuNavigation.onOpenWindow += OnOpenWindow;
            MenuNavigation.onAddButton += AddButton;
        }

        private void OnDisable()
        {
            MenuNavigation.onIsActiveMavigation -= Setup;
            MenuNavigation.onOpenWindow -= OnOpenWindow;
            MenuNavigation.onAddButton -= AddButton;
        }

        private void Setup(bool isActiveNav)
        {
            active = isActiveNav;
            OpenLayer();
        }

        private void OnOpenWindow(GameObject obj)
        {
            OpenLayer();
        }

        private void OpenLayer()
        {
            if (active)
            {
                int layerNum = MenuNavigation.Instance.layers.Count - 1;
                foreach (Button button in MenuNavigation.Instance.layers[layerNum].buttons)
                {
                    AddButton(button);
                }
            }
        }

        private void AddButton(Button button)
        {
            PointerEnterHelperNav helper = button?.GetComponent<PointerEnterHelperNav>();

            if (!helper)
                helper = button.gameObject.AddComponent<PointerEnterHelperNav>();

            helper.Init(button, this);
        }

        public void PointerEnterCallback()
        {
            PointEnterEvent?.Invoke();
            onPointEnter?.Invoke();
        }

        public void PointerClickCallback(Button button)
        {
            PointClickEvent?.Invoke();
            onPointClick?.Invoke();

            if (button.interactable)
            {
                PointClickEnterEvent?.Invoke();
                onPointClickEnter?.Invoke();
            }
            else
            {
                PointClickNoEnterEvent?.Invoke();
                onPointClickNoEnter?.Invoke();
            }
        }

        public void PointerExitCallback()
        {
            PointExitEvent?.Invoke();
            onPointExit?.Invoke();
        }
    }
}