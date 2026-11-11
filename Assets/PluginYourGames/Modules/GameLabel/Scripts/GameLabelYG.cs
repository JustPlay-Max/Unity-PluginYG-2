using UnityEngine;
using UnityEngine.Events;
using YG.Insides;
using YG.Utils;
using YG.Translations;

namespace YG
{
    public class GameLabelYG : MonoBehaviour
    {
        [Header(GameLabelTranslations.SerializeHeader)]
        [Space(5)]
        [Tooltip(GameLabelTranslations.NotSupportedTooltip)]
        public GameObject notSupported;


        [Tooltip(GameLabelTranslations.DoneTooltip)]
        public GameObject done;

        [Tooltip(GameLabelTranslations.ShowDialogTooltip)]
        public GameObject showDialog;

        [HeaderYG(GameLabelTranslations.EventsHeader)]
        [Space(5)]
        [SerializeField] private UnityEvent onPromptSuccess;
        [Space(5)]
        [SerializeField] private UnityEvent onPromptFail;

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