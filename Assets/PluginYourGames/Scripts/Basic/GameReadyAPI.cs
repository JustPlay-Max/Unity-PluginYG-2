namespace YG
{
    public partial interface IPlatformsYG2
    {
        void GameReadyAPI() { }
    }

    public partial class YG2
    {
        private static bool gameReadyDone;

        [StartYG]
        private static void InitGRA()
        {
            if (infoYG.Basic.autoGRA)
            {
                if (isSDKEnabled)
                {
                    GameReadyAPI();
                }
                else
                {
                    onGetSDKData += InvokeGameReadyAPI;
                }
            }
        }

        public static void GameReadyAPI()
        {
            if (!gameReadyDone)
            {
#if UNITY_EDITOR
                if (!infoYG.Basic.autoGRA)
                    Message("Game Ready API (manual call)");
#endif
                gameReadyDone = true;
                iPlatform.GameReadyAPI();
            }
        }

        private static void InvokeGameReadyAPI()
        {
            onGetSDKData -= InvokeGameReadyAPI;
            GameReadyAPI();
        }
    }
}
