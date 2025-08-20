using System;

namespace YG.Insides
{
    public partial class OptionalPlatform
    {
        public Action onLoadedInterAdv;
        public Action onClickedInterAdv;

        public void FirstInterAdvShow() => YG2.iPlatform.FirstInterAdvShow();
        public void OtherInterAdvShow() => YG2.iPlatform.OtherInterAdvShow();
        public void LoadInterAdv()
        {
#if !UNITY_EDITOR
            YG2.iPlatform.LoadInterAdv();
#endif
        }

        private static bool firstInterAd;
        public static void FirstInterAdvShow_RealizationSkip()
        {
            if (YG2.isFirstGameSession)
            {
                if (!firstInterAd)
                {
                    firstInterAd = true;
                }
                else
                {
                    YG2.InterstitialAdvShow();
                }
            }
            else
            {
                YG2.InterstitialAdvShow();
            }
        }
    }
}