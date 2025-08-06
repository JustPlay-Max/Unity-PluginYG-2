namespace YG
{
    public partial interface IPlatformsYG2
    {
        void GameplayStart() { }
        void GameplayStop() { }
        void HappyTime() { }
    }

    public static partial class YG2
    {
        private static bool gameplaying;
        public static bool isGameplaying { get { return gameplaying; } }
        private static bool saveGameplayState;

        public static void GameplayStart(bool useSaveGameplayState = false)
        {
            if (useSaveGameplayState && (!saveGameplayState || nowAdsShow || !isFocusWindowGame))
                return;

            if (!gameplaying)
            {
                Message("Gameplay Start");
                gameplaying = true;
                iPlatform.GameplayStart();
            }
        }

        public static void GameplayStop(bool useSaveGameplayState = false)
        {
            if (useSaveGameplayState && !nowAdsShow && isFocusWindowGame)
                saveGameplayState = gameplaying;

            if (gameplaying)
            {
                Message("Gameplay Stop");
                gameplaying = false;
                iPlatform.GameplayStop();
            }
        }
    }
}