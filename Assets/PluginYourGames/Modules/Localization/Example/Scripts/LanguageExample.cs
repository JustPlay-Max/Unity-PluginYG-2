using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class LanguageExample : MonoBehaviour
    {
        public string ru, en, tr;

        private Text textComponent;

        private void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        private void OnEnable()
        {
            YG2.onSwitchLang += SwitchLanguage;
            SwitchLanguage(YG2.lang);
        }
        private void OnDisable()
        {
            YG2.onSwitchLang -= SwitchLanguage;
        }

        public void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    textComponent.text = ru;
                    break;
                case "tr":
                    textComponent.text = tr;
                    break;
                default:
                    textComponent.text = en;
                    break;
            }
        }
    }
}