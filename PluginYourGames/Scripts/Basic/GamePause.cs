namespace YG
{
    using System;
    using UnityEngine;

    public static partial class YG2
    {
        public static Action<bool> onPauseGame;
        private static bool pauseGame;
        public static bool isPauseGame { get => pauseGame; }

        public static void PauseGame(bool pause, bool editTimeScale, bool editAudioPause, bool editCursor, bool editEventSystem)
        {
            if (pause == pauseGame)
                return;

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
        public static void PauseGame(bool pause) => PauseGame(pause, true, true, true, infoYG.Basic.editEventSystem);
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