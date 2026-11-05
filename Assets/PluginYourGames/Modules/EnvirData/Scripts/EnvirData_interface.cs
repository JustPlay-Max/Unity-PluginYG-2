namespace YG
{
    using System;
    using UnityEngine;
#if UNITY_WEBGL
    using System.Runtime.InteropServices;
#endif
    using YG.Insides;
    using YG.Utils;

    public partial interface IPlatformsYG2
    {
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern IntPtr GeneralEnvirData_js();
#endif
        void InitEnirData()
        {
#if PLATFORM_WEBGL
            IntPtr ptr = GeneralEnvirData_js();
            string jsonString = Marshal.PtrToStringUTF8(ptr);
            YGInsides.FreeBuffer(ptr);
            YG2.envir = JsonUtility.FromJson<YG2.EnvirData>(jsonString);
#else
            string language = SystemLanguageYG.GetLanguageCode(Application.systemLanguage);
            YG2.envir.language = language;
            YG2.envir.browserLang = language;

            if (Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld)
            {
                YG2.envir.deviceType = "mobile";
                YG2.envir.isMobile = true;
                YG2.envir.isDesktop = false;
            }
#endif
        }

        void GetEnvirData()
        {
            InitEnirData();
            YG2.GetDataInvoke();
        }
    }
}
