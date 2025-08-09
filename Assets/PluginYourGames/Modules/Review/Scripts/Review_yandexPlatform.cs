#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;
using YG.Insides;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern string InitReview_js();

        public void InitReview()
        {
#if !UNITY_EDITOR
            if (InitReview_js() == "true")
                YG2.reviewCanShow = true;
#else
            YG2.reviewCanShow = true;
#endif
        }

        [DllImport("__Internal")]
        private static extern void Review_js();

        public void ReviewShow()
        {
#if !UNITY_EDITOR
            Review_js();
#else
            YGInsides.ReviewSent(true);
#endif
        }
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void ReviewSent(string feedbackSent)
        {
            bool sent = feedbackSent == "true" ? true : false;
            YGInsides.ReviewSent(sent);
        }
    }
}
#endif