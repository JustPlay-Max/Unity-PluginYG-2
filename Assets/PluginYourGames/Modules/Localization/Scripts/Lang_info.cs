using System;
using UnityEngine;
using YG.Translations;

namespace YG
{
    public partial class InfoYG
    {
        public LocalizationSettings Localization = new LocalizationSettings();

        [Serializable]
        public partial class LocalizationSettings
        {
            public enum SetLangMod : byte
            {
#if RU_YG2
                [InspectorName("Только при первом запуске")]
#endif
                FirstLaunchOnly = 0,
#if RU_YG2
                [InspectorName("При каждом запуске")]
#endif
                EveryGameLaunch,
#if RU_YG2
                [InspectorName("Не менять язык при запуске")]
#endif
                DoNotChangeLanguageStartup
            };

            [Tooltip(LocalizationTranslations.SetLanguageModTooltip)]
            [Space(5)]
            public SetLangMod setLanguageMod = SetLangMod.EveryGameLaunch;
        }
    }
}