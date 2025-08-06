using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class EnvirLangExample : MonoBehaviour
    {
        public string ru, en, tr;

        private Text textComponent;

        private void Start()
        {
            textComponent = GetComponent<Text>();

#if EnvirData_yg
            switch (YG2.envir.language)
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
#else
            textComponent.text = "Envir Data not import";
#endif
        }
    }
}