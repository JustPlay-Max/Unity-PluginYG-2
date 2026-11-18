#if UNITY_EDITOR
using System;
using UnityEngine;
using YG.Insides;
using YG.Translations;

namespace YG
{
    public partial class InfoYG
    {
        public ServerTimeSettings ServerTime;

        [Serializable]
        public partial class ServerTimeSettings
        {
            public long serverTime => manualDateSetup ? manualDate : DateTimeOffset.Now.ToUnixTimeMilliseconds();

            [HeaderYG(Langs.simulation, 5)]
            [Tooltip(ServerTimeTranslations.ManualDateSetupTooltip)]
            public bool manualDateSetup = false;

            [NestedYG(nameof(manualDateSetup))]
            [SerializeField, Range(1970, 2050)] private int year = 2025;

            [NestedYG(nameof(manualDateSetup))]
            [SerializeField, Range(1, 12)] private int month = 6;

            [NestedYG(nameof(manualDateSetup))]
            [SerializeField, Range(1, 31)] private int day = 15;

            [NestedYG(nameof(manualDateSetup))]
            [SerializeField, Range(0, 23)] private int hour = 12;

            [NestedYG(nameof(manualDateSetup))]
            [SerializeField, Range(0, 59)] private int minute = 30;

            [NestedYG(nameof(manualDateSetup))]
            [SerializeField, Range(0, 59)] private int second = 30;
            private long manualDate => new DateTimeOffset(new DateTime(year, month, day, hour, minute, second)).ToUnixTimeMilliseconds();
        }
    }
}
#endif