#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern string LangRequest_js();

        public string GetLanguage()
        {
            return LangRequest_js();
        }
    }
}
#endif