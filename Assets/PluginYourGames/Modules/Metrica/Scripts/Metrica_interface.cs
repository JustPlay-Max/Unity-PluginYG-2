#if PLATFORM_WEBGL
using System.Runtime.InteropServices;
#endif

namespace YG
{
    public partial interface IPlatformsYG2
    {
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern bool InitYandexMetrica(string counterId);
#endif
        void InitMetrica()
        {
#if PLATFORM_WEBGL
            if (YG2.infoYG.Metrica.useYandexMetrica)
                InitYandexMetrica(YG2.infoYG.Metrica.metricaCounterID.ToString());
#endif
        }

#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern bool YandexMetricaSend_js(string eventName);
#endif
        void MetricaSend(string eventName)
        {
#if PLATFORM_WEBGL
            if (YG2.infoYG.Metrica.useYandexMetrica)
                YandexMetricaSend_js(eventName);
#endif
        }

#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern bool YandexMetricaSend2_js(string eventName, string eventData);
#endif
        void MetricaSend(string eventName, string eventData)
        {
#if PLATFORM_WEBGL
            if (YG2.infoYG.Metrica.useYandexMetrica)
                YandexMetricaSend2_js(eventName, eventData);
#endif
        }
    }
}
