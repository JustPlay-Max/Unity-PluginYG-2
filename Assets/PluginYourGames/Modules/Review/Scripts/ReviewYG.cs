using UnityEngine;
using UnityEngine.Events;

namespace YG
{
    public class ReviewYG : MonoBehaviour
    {
#if RU_YG2
        [Tooltip("Обновлять информацию при каждой активации объекта (в OnEnable)?")]
#else
        [Tooltip("Update information every time an object is activated (in OnEnable)?")]
#endif
        public bool updateDataOnEnable;
        [Space(15)]
        public UnityEvent ReviewAvailable;
        public UnityEvent ReviewNotAvailable;
        public UnityEvent ReviewSuccess;
        public UnityEvent ReviewFailed;

        private void Awake() => ReviewNotAvailable.Invoke();

        private void OnEnable()
        {
            YG2.onGetSDKData += UpdateData;
            YG2.onReviewSent += ReviewSent;

            if (YG2.isSDKEnabled) UpdateData();
            if (updateDataOnEnable) UpdateData();
        }
        private void OnDisable()
        {
            YG2.onGetSDKData -= UpdateData;
            YG2.onReviewSent -= ReviewSent;
        }

        public void UpdateData()
        {
#if UNITY_EDITOR
            YG2.reviewCanShow = true;
#endif
            if (YG2.reviewCanShow)
                ReviewAvailable.Invoke();
            else ReviewNotAvailable.Invoke();
        }

        private void ReviewSent(bool sent)
        {
            if (sent)
                ReviewSuccess.Invoke();
            else
                ReviewFailed.Invoke();

            ReviewNotAvailable.Invoke();
        }

        public void ReviewShow()
        {
            ReviewNotAvailable.Invoke();
            YG2.reviewCanShow = false;

            YG2.ReviewShow();
        }
    }
}
