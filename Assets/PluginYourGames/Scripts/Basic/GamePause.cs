namespace YG
{
    using System;
    using UnityEngine;

    public static partial class YG2
    {
        public static Action<bool> onPauseGame;
        private static bool pauseGame;
        public static bool isPauseGame { get => pauseGame; }
#if !UNITY_EDITOR && InterstitialAdv_yg && YandexGamesPlatform_yg
        private static bool firstPauseGameForInterAdvEvent;
        private static bool firstPauseGameForInterAdvEventComplete;
#endif
        public static void PauseGame(bool pause, bool editTimeScale, bool editAudioPause, bool editCursor, bool editEventSystem)
        {
            if (pause == pauseGame)
                return;

#if !UNITY_EDITOR && InterstitialAdv_yg && YandexGamesPlatform_yg
            if (!firstPauseGameForInterAdvEventComplete)
            {
                if (!firstPauseGameForInterAdvEvent && pause)
                {
                    if (Time.unscaledTime < 5)
                    {
                        firstPauseGameForInterAdvEvent = true;
                        Insides.YGInsides.OpenInterAdv();
                        return;
                    }
                    else firstPauseGameForInterAdvEventComplete = true;
                }

                if (firstPauseGameForInterAdvEvent && !pause)
                {
                    firstPauseGameForInterAdvEventComplete = true;
                    Insides.YGInsides.CloseInterAdv();
                    return;
                }
            }
#endif
            if (pause)
            {
                GameplayStop(true);
            }
            else
            {
                if (nowAdsShow)
                    return;

                GameplayStart(true);
            }

            pauseGame = pause;
            onPauseGame?.Invoke(pause);

            if (infoYG.Basic.autoPauseGame)
            {
                if (pause)
                {
#if UNITY_EDITOR
                    if (!UnityEditor.EditorApplication.isPlaying ||
                        UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
                        return;
#endif
                    GameObject pauseObj = new GameObject() { name = "PauseGameYG" };
                    MonoBehaviour.DontDestroyOnLoad(pauseObj);
                    PauseGameYG pauseScr = pauseObj.AddComponent<PauseGameYG>();
                    pauseScr.Setup(editTimeScale, editAudioPause, editCursor, editEventSystem);
                }
                else
                {
                    if (PauseGameYG.inst != null)
                        PauseGameYG.inst.PauseDisabled();
                }
            }
        }
        public static void PauseGame(bool pause) => PauseGame(pause, infoYG.Basic.editTimeScale, true, true, infoYG.Basic.editEventSystem);
        public static void PauseGameNoEditEventSystem(bool pause) => PauseGame(pause, true, true, true, false);

    }
}

#if PLATFORM_WEBGL
namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void SetPauseGame(string pause) => YG2.PauseGame(pause == "true");
    }
}
#endif
