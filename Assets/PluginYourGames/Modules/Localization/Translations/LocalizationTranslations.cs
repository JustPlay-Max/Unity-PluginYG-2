namespace YG.Translations
{
    public class LocalizationTranslations
    {
#if RU_YG2
        public const string SetLanguageModTooltip = "Менять язык игры в соответствии с языком платформы:\n\n •  First LaunchOnly - только при первом запуске игры.\n\n •  Every Game Launch - каждый раз при запуске игры.\n\n •  Do Not Change Language Startup - не менять язык при запуске игры.";
#else
        public const string SetLanguageModTooltip = "Change the language of the game according to the language of the platform:\n\n • First LaunchOnly - only at the first launch of the game.\n\n • Every Game Launch - every time the game is launched.\n\n • Do Not Change Language Startup - do not change the language when starting the game.";
#endif
    }
}