using UnityEngine;
using UnityEngine.Events;
using YG.Insides;
using YG.Utils;

namespace YG
{
    public class GameLabelYG : MonoBehaviour
    {
#if RU_YG2
        [HeaderYG("Заполнить")]
        [Tooltip("Объект (отключённая кнопка или текст), который будет сообщать о том, что ярлык не поддерживается. Данный объект можно не указывать, тогда, если ярлык не будет поддерживаться - ничего не будет отображаться.")]
#else
        [Header("Serialize")]
        [Tooltip("An object (disabled button or text) that will indicate that the shortcut is not supported. This object can be omitted, then if the shortcut is not supported, nothing will be displayed.")]
#endif
        public GameObject notSupported;
#if RU_YG2
        [Tooltip("Объект (отключённая кнопка или текст), который будет сообщать о том, что ярлык уже установлен. Данный объект можно не указывать, тогда, если ярлык уже установлен - ничего не будет отображаться.")]
#else
        [Tooltip("An object (a disabled button or text) that will indicate that the shortcut has already been installed. This object can be omitted, then if the shortcut is already installed, nothing will be displayed.")]
#endif
        public GameObject done;
#if RU_YG2
        [Tooltip("Объект с кнопкой, которая будет предлагать установить ярлык на рабочий стол (возможно, за вознаграждение). При клике на кнопку необходимо запускать метод GameLabelShowDialog через данный скрипт или через EventsYG2 скрипт.")]
#else
        [Tooltip("An object with a button that will offer to install a shortcut on the desktop (possibly for a fee). When clicking on the button, you need to run the GameLabelShowDialog method through this script or through the EventsYG2 script.")]
#endif
        public GameObject showDialog;
#if RU_YG2
        [HeaderYG("События")]
#else
        [HeaderYG("Events")]
#endif
        [Space(5)]
        public UnityEvent onPromptSuccess;
        public UnityEvent onPromptFail;

        private void Awake()
        {
            if (notSupported)
                notSupported.SetActive(false);

            if (done)
                done.SetActive(false);

            showDialog.SetActive(false);
        }

        private void OnEnable()
        {
            YG2.onGetSDKData += UpdateData;
            YG2.onGameLabelSuccess += OnGameLabelSuccess;
            YG2.onGameLabelFail += OnGameLabelFail;

            if (YG2.isSDKEnabled)
                UpdateData();
        }
        private void OnDisable()
        {
            YG2.onGetSDKData -= UpdateData;
            YG2.onGameLabelSuccess -= OnGameLabelSuccess;
            YG2.onGameLabelFail -= OnGameLabelFail;
        }

        public void UpdateData()
        {
            if (LocalStorage.GetKey(YGInsides.gameLabelDoneSaveKey) == "true")
            {
                if (notSupported)
                    notSupported.SetActive(false);

                if (done)
                    done.SetActive(true);

                showDialog.SetActive(false);
            }
            else if (!YG2.gameLabelCanShow)
            {
                if (notSupported)
                    notSupported.SetActive(true);

                if (done)
                    done.SetActive(false);

                showDialog.SetActive(false);
            }
            else
            {
                if (notSupported)
                    notSupported.SetActive(false);

                if (done)
                    done.SetActive(false);

                showDialog.SetActive(true);
            }
        }

        public void GameLabelShowDialog() => YG2.GameLabelShowDialog();

        private void OnGameLabelSuccess()
        {
            onPromptSuccess?.Invoke();
            UpdateData();
        }
        private void OnGameLabelFail()
        {
            YG2.gameLabelCanShow = false;
            onPromptFail?.Invoke();
            UpdateData();
        }
    }
}