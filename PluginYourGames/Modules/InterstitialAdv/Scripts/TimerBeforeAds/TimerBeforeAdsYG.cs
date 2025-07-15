using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace YG
{
    public class TimerBeforeAdsYG : MonoBehaviour
    {
#if RU_YG2
        [Tooltip("Объект таймера перед показом рекламы. Он будет активироваться и деактивироваться в нужное время.")]
#else
        [Tooltip("The timer object before the ad is shown. It will activate and deactivate at the right time.")]
#endif
        [SerializeField]
        private GameObject secondsPanelObject;
#if RU_YG2
        [Tooltip("Массив объектов, которые будут показываться по очереди через секунду. Сколько объектов вы поместите в массив, столько секунд будет отчитываться перед показом рекламы.\n\nНапример, поместите в массив три объекта: певый с текстом '3', второй с текстом '2', третий с текстом '1'.\nВ таком случае произойдёт отчет трёх секунд с показом объектов с цифрами перед рекламой.")]
#else
        [Tooltip("An array of objects that will be displayed in turn in a second. How many objects you put in the array will be reported for as many seconds before the ad is shown.\n\nFor example, put three objects in the array: the left with the text '3', the second with the text '2', the third with the text '1'.\nIn this case, a three-second report will occur showing objects with numbers before advertising.")]
#endif
        [SerializeField]
        private GameObject[] secondObjects;

        [Space(20)]
        [SerializeField] private UnityEvent onShowTimer;
        [SerializeField] private UnityEvent onHideTimer;

        private int objSecCounter;
        private Coroutine checkTimerAdCoroutine, timerAdShowCoroutine, backupTimerClosureCoroutine;

        private void OnEnable()
        {
            YG2.onOpenAnyAdv += RestartTimer;
            RestartTimer();
        }

        private void OnDisable()
        {
            YG2.onOpenAnyAdv -= RestartTimer;
        }

        IEnumerator CheckTimerAd()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (YG2.isTimerAdvCompleted && !YG2.nowAdsShow)
                {
                    onShowTimer?.Invoke();
                    objSecCounter = 0;

                    if (secondsPanelObject)
                        secondsPanelObject.SetActive(true);

                    YG2.PauseGame(true);

                    timerAdShowCoroutine = StartCoroutine(TimerAdShow());
                    checkTimerAdCoroutine = null;
                    yield break;
                }
            }
        }

        IEnumerator TimerAdShow()
        {
            while (true)
            {
                if (objSecCounter < secondObjects.Length)
                {
                    for (int i2 = 0; i2 < secondObjects.Length; i2++)
                        secondObjects[i2].SetActive(false);

                    secondObjects[objSecCounter].SetActive(true);
                    objSecCounter++;

                    yield return new WaitForSecondsRealtime(1.0f);
                }

                if (objSecCounter == secondObjects.Length)
                {
                    YG2.InterstitialAdvShow();
                    backupTimerClosureCoroutine = StartCoroutine(BackupTimerClosure());

                    while (!YG2.nowInterAdv)
                        yield return null;

                    RestartTimer();
                    yield break;
                }
            }
        }

        IEnumerator BackupTimerClosure()
        {
            yield return new WaitForSecondsRealtime(2f);

            if (objSecCounter != 0)
            {
                RestartTimer();
                YG2.PauseGame(false);
            }

            backupTimerClosureCoroutine = null;
        }

        private void RestartTimer()
        {
            secondsPanelObject.SetActive(false);
            foreach (var obj in secondObjects)
                obj.SetActive(false);

            onHideTimer?.Invoke();
            objSecCounter = 0;

            if (checkTimerAdCoroutine == null)
            {
                if (secondObjects.Length > 0)
                    checkTimerAdCoroutine = StartCoroutine(CheckTimerAd());
                else
                    Debug.LogError("Fill in the array 'secondObjects'");
            }

            if (timerAdShowCoroutine != null)
            {
                StopCoroutine(timerAdShowCoroutine);
                timerAdShowCoroutine = null;
            }

            if (backupTimerClosureCoroutine != null)
            {
                StopCoroutine(backupTimerClosureCoroutine);
                backupTimerClosureCoroutine = null;
            }
        }
    }
}
