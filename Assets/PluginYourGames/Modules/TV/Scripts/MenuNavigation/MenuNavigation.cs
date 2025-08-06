using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

namespace YG.MenuNav
{
    [DefaultExecutionOrder(-99)]
    public class MenuNavigation : MonoBehaviour
    {
        public Button usButton;
        public GameObject exitGameObj;
        public bool navigationOnDesktop, navigationOnMobile;
        public UnityEvent OnBackButton;
        public List<LayerUI> layers = new List<LayerUI>();

        public static MenuNavigation Instance;
        public static Action<bool> onIsActiveMavigation;
        public static Action<Button> onSelectButton;
        public static Action<GameObject> onOpenWindow;
        public static Action onCloseWindow;
        public static Action onEnterButton;
        public static Action<Button> onAddButton;
        public static Action<Button> onRemoveButton;
        public static Action onUnselectButton;

        private void OnEnable()
        {
            YG2.onTVKeyDown += OnKeyDown;
            YG2.onTVKeyBack += OnKeyBack;
        }

        private void OnDisable()
        {
            YG2.onTVKeyDown -= OnKeyDown;
            YG2.onTVKeyBack -= OnKeyBack;
        }

        private void Start()
        {
            Instance = this;
            bool isActiveMavigation = true;

            if (!YG2.envir.isTV)
            {
                if (navigationOnDesktop && YG2.envir.isDesktop)
                    TVTesting.Create();
                else if (navigationOnMobile && (YG2.envir.isMobile || YG2.envir.isTablet))
                    TVTesting.Create();
                else
                    isActiveMavigation = false;
            }

            if (isActiveMavigation)
            {
                AddLayer(null);

                if (usButton == null)
                    usButton = FindFirstObjectByType<Button>();

                if (usButton != null)
                    SelectButton(usButton);
            }

            onIsActiveMavigation?.Invoke(isActiveMavigation);
        }

        private bool NavigationAllow()
        {
            if (YG2.envir.isDesktop && !navigationOnDesktop)
                return false;
            else if ((YG2.envir.isMobile || YG2.envir.isTablet) && !navigationOnMobile)
                return false;
            else
                return true;
        }

        private void OnKeyDown(string key)
        {
            Button b;
            switch (key)
            {
                case "Up":
                    b = usButton.FindSelectableOnUp()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Left":
                    b = usButton.FindSelectableOnLeft()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Down":
                    b = usButton.FindSelectableOnDown()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Right":
                    b = usButton.FindSelectableOnRight()?.GetComponent<Button>();
                    SelectButton(b);
                    break;
                case "Enter":
                    if (usButton)
                    {
                        usButton.onClick?.Invoke();
                        onEnterButton?.Invoke();
                    }
                    break;
            }
        }

        private void OnKeyBack()
        {
            if (layers.Count > 1)
            {
                CloseWindow();
            }
            else if (exitGameObj && YG2.envir.isTV)
            {
                OpenWindow(exitGameObj);
            }
            else
            {
                OnBackButton.Invoke();
            }
        }

        public void SelectButton(Button button)
        {
            if (button == null || !NavigationAllow())
                return;

            usButton = button;
            onSelectButton?.Invoke(button);
        }

        public void UnselectButton(bool considerDeviceParams = false)
        {
            if (considerDeviceParams)
            {
                if (!YG2.envir.isDesktop || (YG2.envir.isDesktop && !navigationOnDesktop))
                    return;
            }

            onUnselectButton?.Invoke();
        }

        public void SelectFirstButton()
        {
            Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
            usButton = buttons.FirstOrDefault(button => button.enabled);
            SelectButton(usButton);
        }

        private void AddLayer(GameObject openLayerObj)
        {
            if (openLayerObj)
                openLayerObj.SetActive(true);

            LayerUI newLayer = new LayerUI
            {
                layer = openLayerObj,
                buttons = new List<Button>()
            };

            Button[] allButtonsInScene = FindObjectsByType<Button>(FindObjectsSortMode.None);

            foreach (Button button in allButtonsInScene)
            {
                if (button.enabled && button.navigation.mode != Navigation.Mode.None)
                {
                    newLayer.buttons.Add(button);
                }
            }

            layers.Add(newLayer);
        }

        public void OpenWindow(GameObject openLayerObj)
        {
            if (!NavigationAllow())
                return;

            foreach (Button button in layers[layers.Count - 1].buttons)
                button.enabled = false;

            AddLayer(openLayerObj);
            SelectFirstButton();

            onOpenWindow?.Invoke(openLayerObj);
        }

        public void CloseWindow()
        {
            if (!NavigationAllow())
                return;

            onCloseWindow?.Invoke();

            if (layers.Count <= 1)
                return;

            int usLayer = layers.Count - 1;
            layers[usLayer].layer.SetActive(false);

            usLayer--;

            for (int i = 0; i < layers[usLayer].buttons.Count; i++)
            {
                if (layers[usLayer].buttons[i])
                    layers[usLayer].buttons[i].enabled = true;
            }

            layers.RemoveAt(usLayer + 1);
            SelectFirstButton();
        }

        public void AddButtonList(int layer, Button button)
        {
            if (layer < layers.Count && button && !layers[layer].buttons.Contains(button))
                layers[layer].buttons.Add(button);
            onAddButton?.Invoke(button);
        }

        public void AddButtonList(Button button)
        {
            AddButtonList(0, button);
        }

        public void RemoveButtonList(int layer, Button button)
        {
            onRemoveButton?.Invoke(button);
            if (layer < layers.Count && layers[layer].buttons.Contains(button))
                layers[layer].buttons.Remove(button);
        }

        public void RemoveButtonList(Button button)
        {
            RemoveButtonList(0, button);
        }

        [Serializable]
        public struct LayerUI
        {
            public GameObject layer;
            public List<Button> buttons;
        }
    }
}