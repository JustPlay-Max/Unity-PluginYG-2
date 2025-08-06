using System;
using YG.Utils;
using YG.Insides;

namespace YG
{
    public partial class YG2
    {
        public static bool gameLabelCanShow;
        public static Action onGameLabelSuccess;
        public static Action onGameLabelFail;

        [InitYG]
        private static void InitGameLabel()
        {
#if UNITY_EDITOR
            // Reset static for ESC
            onGameLabelSuccess = null;
            onGameLabelFail = null;
#endif
#if !UNITY_EDITOR
            iPlatform.GameLabelInit();
#else
            gameLabelCanShow = true;
#endif
        }

        public static void GameLabelShowDialog()
        {
            if (gameLabelCanShow)
            {
#if !UNITY_EDITOR
                iPlatform.GameLabelShowDialog();
#else
                Message("GameLabel show dialog - success simulation");
                YGInsides.OnGameLabelSuccess();
#endif
            }
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public const string gameLabelDoneSaveKey = "YG2_GameLabelDone";

        public static void OnGameLabelSuccess()
        {
            LocalStorage.SetKey(gameLabelDoneSaveKey, "true");
            YG2.onGameLabelSuccess?.Invoke();
            YG2.gameLabelCanShow = false;
        }

        public static void OnGameLabelFail()
        {
            YG2.onGameLabelFail?.Invoke();
            YG2.gameLabelCanShow = false;
        }
    }
}