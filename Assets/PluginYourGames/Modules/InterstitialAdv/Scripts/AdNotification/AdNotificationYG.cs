using System.Collections;
using UnityEngine;

namespace YG
{
    public class AdNotificationYG : MonoBehaviour
    {
#if RU_YG2
        [Tooltip("Объект, который будет активироваться перед открытием рекламы. И деактивироваться при открытии.")]
#else
        [Tooltip("The object that will be activated before opening the ad. And deactivate when opened.")]
#endif
        public GameObject notificationObj;
#if RU_YG2
        [Tooltip("Максимальное время показа объекта заглушки перед рекламой. Если реклама так и не будет показана, то объект скроется через указанное в данном параметре время.")]
#else
        [Tooltip("The maximum time for displaying the stub object before advertising. If the advertisement is not shown, the object will disappear after the time specified in this parameter.")]
#endif
        [Min(0.1f)]
        public float waitingForAds = 1;

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