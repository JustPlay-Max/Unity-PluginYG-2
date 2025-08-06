#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        [DllImport("__Internal")]
        private static extern bool IsInitSDK_js();
        public void InitAwake()
        {
            if (YG2.infoYG.Basic.syncInitSDK)
            {
#if !UNITY_EDITOR
                if (IsInitSDK_js())
                    YG2.SyncInitialization();
#else
                YG2.SyncInitialization();
#endif
            }
        }

        [DllImport("__Internal")]
        private static extern void InitGame_js();
        public void InitComplete()
        {
#if !UNITY_EDITOR
            InitGame_js();
#endif
        }

        [DllImport("__Internal")]
        private static extern void GameReadyAPI_js();

        public void GameReadyAPI()
        {
#if !UNITY_EDITOR
            GameReadyAPI_js();
#endif
        }

        [DllImport("__Internal")]
        private static extern void GameplayStart_js();

        public void GameplayStart()
        {
#if !UNITY_EDITOR
            GameplayStart_js();
#endif
        }

        [DllImport("__Internal")]
        private static extern void GameplayStop_js();

        public void GameplayStop()
        {
#if !UNITY_EDITOR
            GameplayStop_js();
#endif
        }

        [DllImport("__Internal")]
        private static extern void LogStyledMessage(string message);
        public void Message(string message) => LogStyledMessage(message);
    }
}

namespace YG.Insides
{
    public partial class YGSendMessage
    {
        public void InitSDKComplete() => YG2.SyncInitialization();
    }
}
#endif