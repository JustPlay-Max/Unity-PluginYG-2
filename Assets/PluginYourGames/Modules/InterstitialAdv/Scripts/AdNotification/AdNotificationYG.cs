using System.Collections;
using UnityEngine;
using YG.Localization;

namespace YG
{
    public class AdNotificationYG : MonoBehaviour
    {
        [Tooltip(InterstitialAdvLocalization.NotificationObj)]
        public GameObject notificationObj;

        [Tooltip(InterstitialAdvLocalization.WaitingForAds)]
        [Min(0.1f)] public float waitingForAds = 1;

        public static bool isShowNotification;
        public static AdNotificationYG Instance;

        private Coroutine closeNotifCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                YG2.onAdvNotification += OnAdNotification;
                YG2.onOpenAnyAdv += OnOpenAd;
                notificationObj.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            YG2.onAdvNotification -= OnAdNotification;
            YG2.onOpenAnyAdv -= OnOpenAd;
        }

        private void OnAdNotification()
        {
            YG2.PauseGame(true);
            notificationObj.SetActive(true);
            isShowNotification = true;
            closeNotifCoroutine = StartCoroutine(CloseNotification());
        }

        private IEnumerator CloseNotification()
        {
            yield return new WaitForSecondsRealtime(waitingForAds);
            notificationObj.SetActive(false);
            isShowNotification = false;
            YG2.PauseGame(false);
        }

        private void OnOpenAd()
        {
            notificationObj.SetActive(false);
            isShowNotification = false;

            if (closeNotifCoroutine != null)
            {
                StopCoroutine(closeNotifCoroutine);
            }
        }
    }
}