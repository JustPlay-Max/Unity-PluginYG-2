using System;

namespace YG
{
    public partial class YG2
    {
        public static bool reviewCanShow;
        public static Action<bool> onReviewSent;

        [InitYG]
        private static void InitReview()
        {
#if UNITY_EDITOR
            onReviewSent = null; // Reset static for ESC
#endif
            iPlatform.InitReview();
        }

        public static void ReviewShow()
        {
            Message("Review");
            iPlatform.ReviewShow();
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static void ReviewSent(bool feedbackSent)
        {
            YG2.reviewCanShow = false;
            YG2.onReviewSent?.Invoke(feedbackSent);
        }
    }
}