using System;
using UnityEngine;

namespace YG
{
    public partial class InfoYG
    {
        public LocalizationSettings Localization = new LocalizationSettings();

        [Serializable]
        public partial class LocalizationSettings
        {
            public enum SetLangMod
            {
#if RU_YG2
                [InspectorName("Только при первом запуске")]
#endif
                FirstLaunchOnly,
#if RU_YG2
                [InspectorName("При каждом запуске")]
#endif
                EveryGameLaunch,
#if RU_YG2
                [InspectorName("Не менять язык при запуске")]
#endif
                DoNotChangeLanguageStartup
            };
#if RU_YG2
            [Tooltip("Менять язык игры в соответствии с языком платформы:\n\n •  First LaunchOnly - только при первом запуске игры.\n\n •  Every Game Launch - каждый раз при запуске игры.\n\n •  Do Not Change Language Startup - не менять язык при запуске игры.")]
#else
            [Tooltip("Change the language of the game according to the language of the platform:\n\n • First LaunchOnly - only at the first launch of the game.\n\n • Every Game Launch - every time the game is launched.\n\n • Do Not Change Language Startup - do not change the language when starting the game.")]
#endif
            [Space(5)]
            public SetLangMod setLanguageMod = SetLangMod.EveryGameLaunch;
        }
    }
}