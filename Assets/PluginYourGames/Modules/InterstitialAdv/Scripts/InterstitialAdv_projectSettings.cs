#if UNITY_EDITOR
namespace YG.Insides
{
    public partial class ProjectSettings
    {
        public bool showFirstAdv = true;
        public int interAdvInterval = 60;
        public bool postponeCallByFail;

        [ApplySettings]
        private void InterAdv_ApplySettings()
        {
            if (YG2.infoYG.platformToggles.showFirstAdv)
                YG2.infoYG.InterstitialAdv.showFirstAdv = showFirstAdv;

            if (YG2.infoYG.platformToggles.interAdvInterval)
                YG2.infoYG.InterstitialAdv.interAdvInterval = interAdvInterval;

            if (YG2.infoYG.platformToggles.postponeCallByFail)
                YG2.infoYG.InterstitialAdv.postponeCallByFail = postponeCallByFail;
        }
    }

    public partial class PlatformToggles
    {
        public bool showFirstAdv;
        public bool interAdvInterval;
        public bool postponeCallByFail;
    }
}
#endif