using System;
using UnityEngine;
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
            Message("Rewarded Ad Show");

            if (!nowInterAdv && !nowRewardAdv)
            {
                if (id == string.Empty || id == null)
                    id = "null";

                onAdvNotification?.Invoke();
#if !UNITY_EDITOR
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
        private enum RewardAdResult { None, Success, Error };
        private static RewardAdResult rewardAdResult = RewardAdResult.None;

        private static string currentRewardID;
        private static float timeOpenRewardedAdv;
        public static Action rewardCallback = null;

        public static void OpenRewardedAdv()
        {
            YG2.PauseGame(true);
            YG2.onOpenRewardedAdv?.Invoke();
            YG2.onOpenAnyAdv?.Invoke();
            YG2.nowRewardAdv = true;
            timeOpenRewardedAdv = Time.realtimeSinceStartup;
        }

        public static void CloseRewardedAdv()
        {
            YG2.nowRewardAdv = false;

            YG2.onCloseRewardedAdv?.Invoke();
            YG2.onCloseAnyAdv?.Invoke();
            YG2.PauseGame(false);

            if (rewardAdResult == RewardAdResult.Success)
            {
                OnRewardedEvent();
            }
            else if (rewardAdResult == RewardAdResult.Error)
            {
                ErrorRewardedAdv();
            }

            rewardAdResult = RewardAdResult.None;
        }

        public static void RewardAdv(string id)
        {
            if (id == "null" || id == null)
                id = string.Empty;

            currentRewardID = id;
#if UNITY_EDITOR
            if (infoYG.Simulation.testFailAds)
                timeOpenRewardedAdv += Time.realtimeSinceStartup + 1;
            else
                timeOpenRewardedAdv = 0;
#endif
            rewardAdResult = RewardAdResult.None;

            if (Time.realtimeSinceStartup > timeOpenRewardedAdv + 0.5f)
            {
                if (infoYG.RewardedAdv.rewardedAfterClosing)
                {
                    rewardAdResult = RewardAdResult.Success;
                }
                else
                {
                    OnRewardedEvent();
                }
            }
            else
            {
                if (infoYG.RewardedAdv.rewardedAfterClosing)
                    rewardAdResult = RewardAdResult.Error;
                else
                    ErrorRewardedAdv();
            }
        }

        private static void OnRewardedEvent()
        {
            Message("Reward Adv");
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