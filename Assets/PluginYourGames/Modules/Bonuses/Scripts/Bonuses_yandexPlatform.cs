#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern void ConsumeBonus_js(string token);

        public void ConsumeBonus(string token)
        {
            ConsumeBonus_js(token);
        }

        [DllImport("__Internal")]
        private static extern void GetBonusCatalog_js();

        public void GetBonusCatalog()
        {
            GetBonusCatalog_js();
        }

        [DllImport("__Internal")]
        private static extern void GetPlayerBonuses_js();

        public void GetPlayerBonuses()
        {
            GetPlayerBonuses_js();
        }
    }
}
#endif