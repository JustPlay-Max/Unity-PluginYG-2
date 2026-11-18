using System;
using UnityEngine;
using YG.Translations;

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
            [Tooltip(MetricaLocalization.UseYandexMetricaTooltip)]
            public bool useYandexMetrica = true;
        }
    }
}