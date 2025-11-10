#if UNITY_EDITOR
namespace YG.Insides
{
    public partial class ProjectSettings
    {
        public InfoYG.LocalizationSettings.SetLangMod setLanguageMod = InfoYG.LocalizationSettings.SetLangMod.EveryGameLaunch;

        [ApplySettings]
        private void Localization_ApplySettings()
        {
            if (YG2.infoYG.platformToggles.setLanguageMod)
                YG2.infoYG.Localization.setLanguageMod = setLanguageMod;
        }
    }

    public partial class PlatformToggles
    {
        public bool setLanguageMod;
    }
}
#endif