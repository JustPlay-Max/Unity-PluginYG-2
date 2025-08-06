#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern void InterstitialAdvShow_js();

        public void InterstitialAdvShow()
        {
            InterstitialAdvShow_js();
        }
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void OpenInterAdv()
        {
            YGInsides.OpenInterAdv();
        }

        public void CloseInterAdv(string wasShown)
        {
            YGInsides.CloseInterAdv(wasShown);
        }
        public void CloseInterAdv() => CloseInterAdv("true");

        public void ResetTimerInterAdv()
        {
            YGInsides.ResetTimerInterAdv();
        }

        public void ErrorInterAdv()
        {
            YGInsides.ErrorInterAdv();
        }
    }
}
#endif