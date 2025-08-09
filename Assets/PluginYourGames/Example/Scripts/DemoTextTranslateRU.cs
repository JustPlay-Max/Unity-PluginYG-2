using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class DemoTextTranslateRU : MonoBehaviour
    {
        [TextArea(1, 100)]
        public string textRU;

#if RU_YG2
        void Start()
        {
            if (textRU != string.Empty)
                GetComponent<Text>().text = textRU;
        }
#endif
    }
}
