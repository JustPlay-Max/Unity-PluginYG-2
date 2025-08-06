using System;
using UnityEngine;

namespace YG
{
    public partial class InfoYG
    {
        public MetricaSettings Metrica = new MetricaSettings();

        [Serializable]
        public partial class MetricaSettings
        {
            public bool enable;

            [NestedYG(nameof(enable)), Min(0)]
            public int metricaCounterID;

            [NestedYG(nameof(enable))]
            public bool log = true;

            [NestedYG(nameof(enable))]
#if RU_YG2
            [Tooltip("По умолчанию используется Яндекс Метрика для всех платформ (Яндекс Метрика работает только на WebGL). Возможно, для корректной работы потребуются добавить хост Яндекс Метрики в белый список для публикации игры на опеределённых площадках. Уточните этот вопрос для каждой площадки. Эта опция дублируется для отдельных настроек каждой платформы, то есть можно её выключить для определённых платформ. Чтобы заменить Яндекс Метрику на другой аналитический сервис, требуется создать скрипт с новой реализацией для конкретного сервиса. Новая реализация полностью заменит Яндекс Метрику и данная опция не будет иметь никакого значения.")]
#else
            [Tooltip("By default, Yandex.Metrica is used for all platforms (Yandex.Metrica only works on WebGL), but it will not work on all platforms. It is possible that in order to work correctly, you will need to add the Yandex.Metrica host to the whitelist for publishing the game on certain sites. This option is duplicated for the individual settings of each platform. To replace Yandex.Metrica with another analytical service, you need to create a script with a new implementation for a specific service. The new implementation will completely replace Yandex.Metrica and this option will have no value.")]
#endif
            public bool useYandexMetrica = true;
        }
    }
}