namespace YG.Insides
{
    public partial class OptionalPlatform
    {
        public void FirstInterAdvShow() => YG2.iPlatform.FirstInterAdvShow();
        public void OtherInterAdvShow() => YG2.iPlatform.OtherInterAdvShow();
        public void LoadInterAdv() => YG2.iPlatform.LoadInterAdv();

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