using System;
using YG.Utils;

namespace YG
{
    public partial class YG2
    {
        private const string LANG_KEY = "langYG";

        public static string lang = "ru";
        public static Action<string> onSwitchLang;
        public static Action<string> onCorrectLang;

        [InitYG_1]
        private static void InitLang()
        {
#if UNITY_EDITOR
            onSwitchLang = null;  // Reset static for ESC
#endif
            if (infoYG.Localization.setLanguageMod == InfoYG.LocalizationSettings.SetLangMod.DoNotChangeLanguageStartup)
            {
                return;
            }

            if (!LocalStorage.HasKey(LANG_KEY))
            {
                GetLanguage();
            }
            else
            {
                if (infoYG.Localization.setLanguageMod == InfoYG.LocalizationSettings.SetLangMod.EveryGameLaunch)
                {
                    GetLanguage();
                }
                else if (infoYG.Localization.setLanguageMod == InfoYG.LocalizationSettings.SetLangMod.FirstLaunchOnly)
                {
                    lang = LocalStorage.GetKey(LANG_KEY);
                }
            }
        }

        [StartYG]
        private static void LangStart()
        {
            onSwitchLang?.Invoke(lang);
        }

        public static void GetLanguage()
        {
#if !UNITY_EDITOR
            lang = iPlatform.GetLanguage();
#else
            string langSim = infoYG.Simulation.language;
            if (langSim != null && langSim != "")
                lang = langSim;
#endif
            lang = lang.ToLower();

            if (lang == "us" || lang == "as" || lang == "ai")
                lang = "en";

            onCorrectLang?.Invoke(lang);

            if (isSDKEnabled)
                onSwitchLang?.Invoke(lang);
        }


        public static void SwitchLanguage(string language)
        {
            lang = language;
            onSwitchLang?.Invoke(language);
            LocalStorage.SetKey(LANG_KEY, lang);
        }
    }
}
