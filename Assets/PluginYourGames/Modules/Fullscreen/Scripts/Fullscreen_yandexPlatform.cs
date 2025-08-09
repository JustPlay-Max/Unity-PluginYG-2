#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern long SetFullscreen_js(bool fullscreen);

        public void SetFullscreen(bool fullscreen)
        {
            SetFullscreen_js(fullscreen);
        }

        [DllImport("__Internal")]
        private static extern bool IsFullscreen_js();

        public bool IsFullscreen()
        {
            return IsFullscreen_js();
        }
    }
}
#endif