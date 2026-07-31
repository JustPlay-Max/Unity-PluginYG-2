using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using YG.Localization;

namespace YG
{
    public class TimerBeforeAdsYG : MonoBehaviour
    {
        private const float TimeBetweenSetActiveObject = 1.0f;

        [Tooltip(InterstitialAdvLocalization.SecondsPanelObject)]
        [SerializeField] private GameObject secondsPanelObject;

        [Tooltip(InterstitialAdvLocalization.SecondObjects)]
        [SerializeField] private GameObject[] secondObjects;

        [Space(20)]
        [SerializeField] private UnityEvent onShowTimer;
        [SerializeField] private UnityEvent onHideTimer;

        private Coroutine _timerRoutine = default;

        private void OnEnable()
        {
            StopRoutine();

            _timerRoutine = StartCoroutine(TimerProcess());
        }

        private void OnDisable()
            => StopRoutine();

        private void StopRoutine()
        {
            if (_timerRoutine != null)
            {
                StopCoroutine(_timerRoutine);
                _timerRoutine = null;
            }
        }

        private IEnumerator TimerProcess()
        {
            var waiting = new WaitForSeconds(YG2.interAdvInterval);

            while (true)
            {
                yield return waiting;

                onShowTimer?.Invoke();

                secondsPanelObject.SetActive(true);

                for (var i = 0; i < secondObjects.Length; i++)
                {
                    secondObjects[i].SetActive(true);

                    yield return new WaitForSecondsRealtime(TimeBetweenSetActiveObject);

                    secondObjects[i].SetActive(false);
                }

                onHideTimer?.Invoke();

                secondsPanelObject.SetActive(false);

                YG2.InterstitialAdvShow();
            }
        }
    }
}
