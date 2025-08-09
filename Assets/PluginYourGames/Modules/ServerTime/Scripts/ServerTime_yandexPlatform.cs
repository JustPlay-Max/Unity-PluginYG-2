#if YandexGamesPlatform_yg
using System;
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern IntPtr ServerTime_js();

        public long ServerTime()
        {
#if UNITY_EDITOR
            return YG2.infoYG.ServerTime.serverTime;
#else
            IntPtr serverTimePtr = ServerTime_js();
            string serverTimeStr = Marshal.PtrToStringAuto(serverTimePtr);
            if (long.TryParse(serverTimeStr, out long serverTime))
            {
                return serverTime;
            }
            return 0;
#endif
        }
    }
}
#endif