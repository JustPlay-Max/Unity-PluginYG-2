using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using YG.Insides;

namespace YG
{
    public class EventsYG2 : MonoBehaviour
    {
        public enum EventYG2Type
        {
            GetSDKData,
            PauseStopGame,
            PauseResumeGame,

            OpenInterAdv,
            CloseInterAdv,
            ErrorInterAdv,

            OpenRewardedAdv,
            CloseRewaededAdv,
            RewardAdv,
            ErrorRewardedAdv,

            AuthTrue,
            AuthFalse,

            PurchaseSuccess,
            PurchaseFailed,

            ReviewSuccess,
            ReviewFailed,

            GameLabelSuccess,
            GameLabelFail
        }

        [Serializable]
        public class TriggerEvent : UnityEvent { }

        [Serializable]
        public class Entry
        {
            public EventYG2Type eventID = EventYG2Type.GetSDKData;
            public TriggerEvent callback = new TriggerEvent();
        }

        [FormerlySerializedAs("delegates")]
        [SerializeField]
        private List<Entry> m_Delegates;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Please use triggers instead (UnityUpgradable) -> triggers", true)]
        public List<Entry> delegates { get { return triggers; } set { triggers = value; } }

        public List<Entry> triggers
        {
            get
            {
                if (m_Delegates == null)
                    m_Delegates = new List<Entry>();
                return m_Delegates;
            }
            set { m_Delegates = value; }
        }

        private void Execute(EventYG2Type id)
        {
            var triggerCount = triggers.Count;

            for (int i = 0, imax = triggers.Count; i < imax; ++i)
            {
                var ent = triggers[i];
                if (ent.eventID == id)
                    ent.callback?.Invoke();
            }
        }

        private void OnEnable()
        {
            YG2.onGetSDKData += OnGetSDKData;
            YG2.onPauseGame += OnPauseGame;
#if InterstitialAdv_yg
            YG2.onOpenInterAdv += OnOpenInterAdv;
            YG2.onCloseInterAdv += OnCloseInterAdv;
            YG2.onErrorInterAdv += OnErrorInterAdv;
#endif
#if RewardedAdv_yg
            YG2.onOpenRewardedAdv += OnOpenRewardedAdv;
            YG2.onCloseRewardedAdv += OnCloseRewaededAdv;
            YG2.onRewardAdv += OnRewardAdv;
            YG2.onErrorRewardedAdv += OnErrorRewardedAdv;
#endif
#if Payments_yg
            YG2.onPurchaseSuccess += OnPurchaseSuccess;
            YG2.onPurchaseFailed += OnPurchaseFailed;
#endif
#if Review_yg
            YG2.onReviewSent += OnReviewSent;
#endif
#if GameLabel_yg
            YG2.onGameLabelSuccess += OnGameLabelSuccess;
            YG2.onGameLabelFail += OnGameLabelFail;
#endif
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= OnGetSDKData;
            YG2.onPauseGame -= OnPauseGame;
#if InterstitialAdv_yg
            YG2.onOpenInterAdv -= OnOpenInterAdv;
            YG2.onCloseInterAdv -= OnCloseInterAdv;
            YG2.onErrorInterAdv -= OnErrorInterAdv;
#endif
#if RewardedAdv_yg
            YG2.onOpenRewardedAdv -= OnOpenRewardedAdv;
            YG2.onCloseRewardedAdv -= OnCloseRewaededAdv;
            YG2.onRewardAdv -= OnRewardAdv;
            YG2.onErrorRewardedAdv -= OnErrorRewardedAdv;
#endif

#if Payments_yg
            YG2.onPurchaseSuccess -= OnPurchaseSuccess;
            YG2.onPurchaseFailed -= OnPurchaseFailed;
#endif
#if Review_yg
            YG2.onReviewSent -= OnReviewSent;
#endif
#if GameLabel_yg
            YG2.onGameLabelSuccess -= OnGameLabelSuccess;
            YG2.onGameLabelFail -= OnGameLabelFail;
#endif
        }

        public void _GameReadyAPI() => YG2.GameReadyAPI();
        public void _GameplayStart() => YG2.GameplayStart();
        public void _GameplayStop() => YG2.GameplayStop();
        public void _HappyTime() => YG2.optionalPlatform.HappyTime();
        public void _PauseGame(bool pause) => YG2.PauseGame(pause);
        public void _PauseGameNoEditEventSystem(bool pause) => YG2.PauseGameNoEditEventSystem(pause);

        private void OnGetSDKData()
        {
            Execute(EventYG2Type.GetSDKData);
#if Authorization_yg
            if (YG2.player.auth)
                Execute(EventYG2Type.AuthTrue);
            else
                Execute(EventYG2Type.AuthFalse);
#endif
        }
        private void OnPauseGame(bool pause)
        {
            if (pause) Execute(EventYG2Type.PauseStopGame);
            else Execute(EventYG2Type.PauseResumeGame);
        }

#if InterstitialAdv_yg
        public void _InterstitialAdvShow() => YG2.InterstitialAdvShow();
        public void _ResetTimerInterAdv() => YGInsides.ResetTimerInterAdv();
        public void _FirstInterAdvShow_optionalPlatform() => YG2.optionalPlatform.FirstInterAdvShow();
        public void _OtherInterAdvShow_optionalPlatform() => YG2.optionalPlatform.OtherInterAdvShow();
        public void _LoadInterAdv_optionalPlatform() => YG2.optionalPlatform.LoadInterAdv();

        private void OnOpenInterAdv() => Execute(EventYG2Type.OpenInterAdv);
        private void OnCloseInterAdv() => Execute(EventYG2Type.CloseInterAdv);
        private void OnErrorInterAdv() => Execute(EventYG2Type.ErrorInterAdv);
#endif
#if RewardedAdv_yg
        public void _RewardedAdvShow(string id) => YG2.RewardedAdvShow(id);
        public void _LoadRewardedAdv_optionalPlatform() => YG2.optionalPlatform.LoadRewardedAdv();

        private void OnOpenRewardedAdv() => Execute(EventYG2Type.OpenRewardedAdv);
        private void OnCloseRewaededAdv() => Execute(EventYG2Type.CloseRewaededAdv);
        private void OnRewardAdv(string id) => Execute(EventYG2Type.RewardAdv);
        private void OnErrorRewardedAdv() => Execute(EventYG2Type.ErrorRewardedAdv);
#endif
#if StickyAdv_yg
        public void _StickyAdActivity(bool activity) => YG2.StickyAdActivity(activity);
#endif
#if Authorization_yg
        public void _GetAuth() => YG2.GetAuth();
        public void _OpenAuthDialog() => YG2.OpenAuthDialog();
#endif
#if EnvirData_yg
        public void _GetEnvirData() => YG2.GetEnvirData();
#endif
#if Localization_yg
        public void _GetLanguage() => YG2.GetLanguage();
        public void _SwitchLanguage(string language) => YG2.SwitchLanguage(language);
#endif
#if Payments_yg
        public void _BuyPayments(string id) => YG2.BuyPayments(id);
        public void _ConsumePurchases() => YG2.ConsumePurchases();

        private void OnPurchaseSuccess(string id) => Execute(EventYG2Type.PurchaseSuccess);
        private void OnPurchaseFailed(string id) => Execute(EventYG2Type.PurchaseFailed);
#endif
#if Storage_yg
        public void _SetDefaultSaves() => YG2.SetDefaultSaves();
        public void _SaveProgress() => YG2.SaveProgress();
        public void _LoadProgress() => YGInsides.LoadProgress();
#endif
#if Fullscreen_yg
        public void _SetFullscreen(bool fullscreen) => YG2.SetFullscreen(fullscreen);
#endif
#if OpenURL_yg
        public void _OnURLDefineDomain(string url) => YG2.OnURLDefineDomain(url);
        public void _OnURL(string url) => YG2.OnURL(url);
        public void _OnDeveloperURL() => YG2.OnDeveloperURL();
        public void _OnGameURL(int appID) => YG2.OnGameURL(appID);
#endif
#if Review_yg
        public void _ReviewShow() => YG2.ReviewShow();

        private void OnReviewSent(bool complete)
        {
            if (complete)
                Execute(EventYG2Type.ReviewSuccess);
            else
                Execute(EventYG2Type.ReviewFailed);
        }
#endif
#if GameLabel_yg
        public void _GameLabelShowDialog() => YG2.GameLabelShowDialog();

        private void OnGameLabelSuccess() => Execute(EventYG2Type.GameLabelSuccess);
        private void OnGameLabelFail() => Execute(EventYG2Type.GameLabelFail);
#endif
    }
}
