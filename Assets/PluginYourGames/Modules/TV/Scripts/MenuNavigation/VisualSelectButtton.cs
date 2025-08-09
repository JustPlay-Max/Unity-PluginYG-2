using UnityEngine;
using UnityEngine.UI;

namespace YG.MenuNav
{
    public class VisualSelectButtton : MonoBehaviour
    {
        public GameObject visualSelectorPrefab;
        public float sizeFrame = 10.0f;
        public bool visualOnDesktop = true, visualOnMobile;

        private RectTransform visualSelector;
        private bool active;

        private void Awake()
        {
            VisualSelector().gameObject.SetActive(false);
        }

        private RectTransform VisualSelector()
        {
            if (visualSelector)
            {
                return visualSelector;
            }
            else
            {
                GameObject visualSelObj = Instantiate(visualSelectorPrefab);
                visualSelector = visualSelObj.GetComponent<RectTransform>();
                return visualSelector;
            }
        }

        private void OnEnable()
        {
            if (!CheckDevice())
                return;

            MenuNavigation.onIsActiveMavigation += Setup;
            MenuNavigation.onSelectButton += OnSelectButton;
            MenuNavigation.onUnselectButton += HideVusualSlector;
        }

        private void OnDisable()
        {
            if (!CheckDevice())
                return;

            MenuNavigation.onIsActiveMavigation -= Setup;
            MenuNavigation.onSelectButton -= OnSelectButton;
            MenuNavigation.onUnselectButton -= HideVusualSlector;
        }

        private void Setup(bool active)
        {
            this.active = active;
        }

        private void OnSelectButton(Button button)
        {
            if (active)
            {
                VisualSelector().gameObject.SetActive(true);
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                CopyTransform(VisualSelector(), buttonRect);
                IncreaseSize(VisualSelector());
            }
        }

        private void CopyTransform(RectTransform target, RectTransform source)
        {
            target.SetParent(source);
            target.anchorMin = new Vector2(0, 0);
            target.anchorMax = new Vector2(1, 1);
            target.position = Vector3.zero;
            target.sizeDelta = Vector2.zero;
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
            target.localScale = Vector2.one;
        }

        private void IncreaseSize(RectTransform target)
        {
            Vector2 newSize = target.sizeDelta;
            newSize.x += sizeFrame;
            newSize.y += sizeFrame;
            target.sizeDelta = newSize;
        }

        public void HideVusualSlector()
        {
            VisualSelector().gameObject.SetActive(false);
            VisualSelector().SetParent(null);
        }

        private bool CheckDevice()
        {
            if (!visualOnDesktop && YG2.envir.isDesktop)
                return false;
            else if (!visualOnMobile && (YG2.envir.isMobile || YG2.envir.isTablet))
                return false;
            return true;
        }
    }
}