#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern string InitGameLabe_js();

        public void GameLabelInit()
        {
            if (InitGameLabe_js() == "true")
                YG2.gameLabelCanShow = true;
        }

        [DllImport("__Internal")]
        private static extern void GameLabelShowDialog_js();

        public void GameLabelShowDialog()
        {
            GameLabelShowDialog_js();
        }
    }

}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void OnGameLabelSuccess() => YGInsides.OnGameLabelSuccess();

        public void OnGameLabelFail() => YGInsides.OnGameLabelFail();
    }
}
#endif