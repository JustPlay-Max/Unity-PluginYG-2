
namespace YG
{
    public static partial class YG2
    {
#if UNITY_EDITOR
        private static bool isFullscreenEditor;
#endif
        public static void SetFullscreen(bool fullscreen)
        {
            if (isFullscreen != fullscreen)
            {
                Message("Set Fullscreen: " + fullscreen);
#if UNITY_EDITOR
                isFullscreenEditor = fullscreen;
#else
                iPlatform.SetFullscreen(fullscreen);
#endif
            }
        }

        public static bool isFullscreen
        {
            get
            {
#if UNITY_EDITOR
                return isFullscreenEditor;
#else
                return iPlatform.IsFullscreen();
#endif
            }
        }
    }
}