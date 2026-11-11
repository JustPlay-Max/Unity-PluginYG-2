namespace YG.Translations
{
    public class PlayerStatsTranslations
    {
#if RU_YG2
        public const string LoadStateErrorMsg = "Состояние не может быть загружено. SDK ещё не был инициализирован!";
        public const string SaveStateErrorMsg = "Состояние не может быть сохранено. SDK ещё не был инициализирован!";
#else
        public const string LoadStateErrorMsg = "State cannot be load. The SDK has not been initialized yet!";
        public const string SaveStateErrorMsg = "State cannot be saved. The SDK has not been initialized yet!";
#endif
    }
}