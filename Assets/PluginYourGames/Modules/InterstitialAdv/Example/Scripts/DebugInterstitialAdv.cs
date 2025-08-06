using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class DebugInterstitialAdv : MonoBehaviour
    {
        public Text timerText;

        private void Update()
        {
#if RU_YG2
            string translate = "Таймер до рекламы: ";
#else
            string translate = "Timer before adv: ";
#endif
            timerText.text = translate + YG2.timerInterAdv.ToString("00.0");
        }
    }
}
