using System;

namespace YG
{
    public partial class YG2
    {
        public static Action<string> onTVKeyDown, onTVKeyUp;
        public static Action onTVKeyBack;

        [InitYG_1]
        private static void CreateTVTestObj()
        {
#if UNITY_EDITOR
            if (!envir.isTV)
                return;
#else
            if (envir.payload != "tvtest")
                return;

            envir.deviceType = "tv";
            envir.isTV = true;
            envir.isMobile = false;
            envir.isTablet = false;
            envir.isDesktop = false;
#endif
            TVTesting.Create();
        }

        public static void ExitTVGame()
        {
#if !UNITY_EDITOR
            iPlatform.ExitTVGame();
#else 
            Message("Exit TV Game");
#endif
        }
    }
}

namespace YG.Insides
{
    public static partial class YGInsides
    {
        public static void TVKeyDown(string key)
        {
            if (key != null || key != "")
                YG2.onTVKeyDown?.Invoke(key);
        }

        public static void TVKeyUp(string key)
        {
            if (key != null || key != "")
                YG2.onTVKeyUp?.Invoke(key);
        }

        public static void TVKeyBack()
        {
            YG2.onTVKeyBack?.Invoke();
        }
    }
}