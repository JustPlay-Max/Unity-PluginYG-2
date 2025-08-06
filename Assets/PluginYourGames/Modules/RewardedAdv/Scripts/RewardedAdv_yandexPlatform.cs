#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern void RewardedAdvShow_js(string id);
        public void RewardedAdvShow(string id) => RewardedAdvShow_js(id);
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void OpenRewardedAdv() => YGInsides.OpenRewardedAdv();

        public void CloseRewardedAdv() => YGInsides.CloseRewardedAdv();

        public void RewardAdv(string id) => YGInsides.RewardAdv(id);

        public void ErrorRewardedAdv() => YGInsides.ErrorRewardedAdv();
    }
}
#endif