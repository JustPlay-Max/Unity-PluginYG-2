using System;
using YG.Insides;
using UnityEngine;

namespace YG
{
    public partial class YG2
    {
        public static Action onOpenInterAdv;
        public static Action onCloseInterAdv;
        public static Action<bool> onCloseInterAdvWasShow;
        public static Action onErrorInterAdv;

        public static float timerInterAdv
        {
            get
            {
                float result = YGInsides.timeShowInterAdv - Time.realtimeSinceStartup;
                result = Mathf.Clamp(result, 0f, interAdvInterval);
                return result;
            }
        }

        public static bool isTimerAdvCompleted
        {
            get
            {
                if (timerInterAdv <= 0)
                    return true;
                return false;
            }
        }

        public static int interAdvInterval
        {
            get
            {
#if UNITY_EDITOR
                return infoYG.Simulation.advIntervalSimulation;
#else
                return infoYG.InterstitialAdv.interAdvInterval;
#endif
            }
        }

        private static bool skipIterAdv;

        [InitYG]
        private static void InitInterstitialAdv()
        {
#if UNITY_EDITOR
            // Reset static for ESC
            YGInsides.timeShowInterAdv = 0;
            skipIterAdv = false;
            onOpenInterAdv = null;
            onCloseInterAdv = null;
            onCloseInterAdvWasShow = null;
            onErrorInterAdv = null;
#endif

#if YandexGamesPlatform_yg
            if (infoYG.InterstitialAdv.showFirstAdv)
                YGInsides.SetTimerInterAdv();
#else
#if UNITY_EDITOR
            if (infoYG.InterstitialAdv.showFirstAdv)
                YGInsides.SetTimerInterAdv();
#else
            if (infoYG.InterstitialAdv.showFirstAdv)
                InterstitialAdvShow();
#endif
#endif
        }

        public static void InterstitialAdvShow()
        {
            if (skipIterAdv)
            {
                skipIterAdv = false;
#if RU_YG2
                Message("Вызов рекламы пропущен, т.к. был выполнен метод SkipNextInterAdCall()");
#else
                Message("The ad call was skipped because the SkipNextInterAdCall() method was executed");
#endif
                return;
            }

            if (!nowAdsShow && isTimerAdvCompleted)
            {
                onAdvNotification?.Invoke();
#if UNITY_EDITOR
                Message("Interstitial Adv");
                if (infoYG.Simulation.testFailAds)
                {
                    Message("Error Interstitial Adv simulation");
                    YGInsides.ErrorInterAdv();
                }
                AdvCallingSimulation.InterstitialAdvOpen();
#else
                iPlatform.InterstitialAdvShow();
#endif
            }
            else
            {
#if RU_YG2
                string message = "Реклама будет доступна через:";
#else
                string message = "The advertisement will be available via:";
#endif
                Message($"{message} {timerInterAdv.ToString("00.0")}");
            }
        }

        public static void SkipNextInterAdCall() => skipIterAdv = true;
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static float timeShowInterAdv;

        public static void OpenInterAdv()
        {
            YG2.PauseGame(true);
            YG2.onOpenInterAdv?.Invoke();
            YG2.onOpenAnyAdv?.Invoke();
            YG2.nowInterAdv = true;
        }

        public static void CloseInterAdv(string wasShown)
        {
            YG2.nowInterAdv = false;
            YG2.onCloseInterAdv?.Invoke();
            YG2.onCloseInterAdvWasShow?.Invoke(wasShown == "true");
            YG2.onCloseAnyAdv?.Invoke();

            if (wasShown == "true")
            {
                SetTimerInterAdv();
            }
            else
            {
                if (infoYG.InterstitialAdv.postponeCallByFail)
                {
                    SetTimerInterAdv(infoYG.InterstitialAdv.postponeCallTimer);
#if RU_YG2
                    string message = "Реклама не была показана. Следующий запрос через:";
#else
                    string message = "The advertisement was not shown. The next request is via:";
#endif
                    Message($"{message} {infoYG.InterstitialAdv.postponeCallTimer}");
                }
                else
                {
#if RU_YG2
                    Message("Реклама не была показана. Ждём следующего запроса.");
#else
                    Message("The advertisement was not shown. We are waiting for the next request.");
#endif
                }
            }
            YG2.PauseGame(false);
        }
        public static void CloseInterAdv(bool wasShown)
        {
            string wasStr = wasShown ? "true" : "false";
            CloseInterAdv(wasStr);
        }
        public static void CloseInterAdv() => CloseInterAdv("true");

        public static void ErrorInterAdv()
        {
            ResetTimerInterAdv();
            YG2.onErrorInterAdv?.Invoke();
            YG2.onErrorAnyAdv?.Invoke();
        }

        public static void ResetTimerInterAdv()
        {
            timeShowInterAdv = Time.realtimeSinceStartup;
        }

        public static void SetTimerInterAdv(int value)
        {
            timeShowInterAdv = Time.realtimeSinceStartup + value;
        }

        public static void SetTimerInterAdv()
        {
            SetTimerInterAdv(YG2.interAdvInterval);
        }
    }
}