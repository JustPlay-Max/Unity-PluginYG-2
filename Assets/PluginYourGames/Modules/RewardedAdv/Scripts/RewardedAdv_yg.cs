using System;
using YG.Insides;

namespace YG
{
    public partial class YG2
    {
        public static Action onOpenRewardedAdv;
        public static Action onCloseRewardedAdv;
        public static Action<string> onRewardAdv;
        public static Action onErrorRewardedAdv;

#if UNITY_EDITOR
        [InitYG]
        private static void RewardedAdvInit()
        {
            // Reset static for ESC
            onOpenRewardedAdv = null;
            onCloseRewardedAdv = null;
            onRewardAdv = null;
            onErrorRewardedAdv = null;
        }
#endif

        public static void RewardedAdvShow(string id)
        {
            if (!nowInterAdv && !nowRewardAdv)
            {
                if (string.IsNullOrEmpty(id)) id = "null";

                YGInsides.currentRewardID = id;
                onAdvNotification?.Invoke();
#if !UNITY_EDITOR
                Message("Rewarded Ad Show");
                iPlatform.RewardedAdvShow(id);
#else
                AdvCallingSimulation.RewardedAdvOpen(id);
#endif
            }
        }

        public static void RewardedAdvShow(string id, Action callback)
        {
            YGInsides.rewardCallback = callback;
            RewardedAdvShow(id);
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static string currentRewardID;
        public static Action rewardCallback = null;

        public static void OpenRewardedAdv()
        {
            YG2.PauseGame(true);
            YG2.onOpenRewardedAdv?.Invoke();
            YG2.onOpenAnyAdv?.Invoke();
            YG2.nowRewardAdv = true;
        }

        public static void CloseRewardedAdv()
        {
            YG2.nowRewardAdv = false;

            YG2.onCloseRewardedAdv?.Invoke();
            YG2.onCloseAnyAdv?.Invoke();
            YG2.PauseGame(false);
        }

        public static void RewardAdv(string id)
        {
            if (id == "null") id = string.Empty;
            currentRewardID = id;
#if UNITY_EDITOR
            if (infoYG.Simulation.testFailAds)
            {
                ErrorRewardedAdv();
            }
            else
            {
                OnRewardedSuccess();
            }
#else
            OnRewardedSuccess();
#endif
        }
        public static void RewardAdv() => RewardAdv(currentRewardID);

        private static void OnRewardedSuccess()
        {
#if UNITY_EDITOR
            Message($"Rewarded Ad completed (Editor log: reward id [{currentRewardID}]");
#else
            Message("Rewarded Ad completed");
#endif
#if InterstitialAdv_yg
            if (YG2.infoYG.RewardedAdv.skipInterAdvAfterReward)
                YG2.SkipNextInterAdCall();
#endif
            if (rewardCallback != null)
            {
                rewardCallback?.Invoke();
                rewardCallback = null;
            }
            YG2.onRewardAdv?.Invoke(currentRewardID);
        }

        public static void ErrorRewardedAdv()
        {
            rewardCallback = null;
            YG2.onErrorRewardedAdv?.Invoke();
            YG2.onErrorAnyAdv?.Invoke();
        }
    }
}