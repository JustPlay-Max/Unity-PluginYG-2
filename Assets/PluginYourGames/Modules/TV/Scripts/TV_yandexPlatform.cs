#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern void ExitTVGame_js();

        public void ExitTVGame()
        {
            ExitTVGame_js();
        }
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void TVKeyDown(string key) => YGInsides.TVKeyDown(key);
        public void TVKeyUp(string key) => YGInsides.TVKeyUp(key);
        public void TVKeyBack() => YGInsides.TVKeyBack();
    }
}
#endif