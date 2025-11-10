using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG.Insides;
using YG.Utils;

namespace YG
{
    [DefaultExecutionOrder(-5000)]
    public static partial class YG2
    {
        public static InfoYG infoYG { get { return InfoYG.Inst(); } }

        public static IPlatformsYG2 iPlatform;
        public static IPlatformsYG2 iPlatformNoRealization;
        public static YGSendMessage sendMessage;
        public static OptionalPlatform optionalPlatform = new OptionalPlatform();
        public static string platform { get => PlatformSettings.currentPlatformBaseName; }
        public static int buildNumber { get => infoYG.Basic.buildNumber; }
        public static bool isSDKEnabled { get => _SDKEnabled; }
        private static bool _SDKEnabled;
        public static bool isFirstGameSession;
        public enum Device { Desktop, Mobile, Tablet, TV }

        private static bool syncInitSDKComplete, awakePassed;

        public static Action onGetSDKData;

        public static bool nowAdsShow
        {
            get
            {
                if (nowInterAdv || nowRewardAdv)
                    return true;
                else
                    return false;
            }
        }
        public static bool nowInterAdv;
        public static bool nowRewardAdv;
        public static Action onAdvNotification, onOpenAnyAdv, onCloseAnyAdv, onErrorAnyAdv;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if UNITY_EDITOR
            // Reset static for ESC
            _SDKEnabled = false;
            sendMessage = null;
            nowInterAdv = false;
            nowRewardAdv = false;
            onGetSDKData = null;
            onAdvNotification = null;
            onOpenAnyAdv = null;
            onCloseAnyAdv = null;
            onErrorAnyAdv = null;
#endif
            if (LocalStorage.GetKey("WasFirstGameSession_YG", "false") == "false")
            {
                LocalStorage.SetKey("WasFirstGameSession_YG", "true");
                isFirstGameSession = true;
            }

            iPlatform = new PlatformYG2();
            iPlatformNoRealization = new PlatformYG2NoRealization();

            GameObject YGObj = new GameObject() { name = "YG2Instance" };
            MonoBehaviour.DontDestroyOnLoad(YGObj);
            sendMessage = YGObj.AddComponent<YGSendMessage>();

            iPlatform.InitAwake();
            awakePassed = true;

            if (!infoYG.Basic.syncInitSDK || syncInitSDKComplete)
            {
                AwakeInit();
            }
        }

        private static void AwakeInit()
        {
            CallAction.CallIByAttribute(typeof(InitYG_0Attribute), typeof(YG2));
            CallAction.CallIByAttribute(typeof(InitYG_1Attribute), typeof(YG2));
            CallAction.CallIByAttribute(typeof(InitYG_2Attribute), typeof(YG2));
            CallAction.CallIByAttribute(typeof(InitYGAttribute), typeof(YG2));
        }

        public static void StartInit()
        {
            if (!_SDKEnabled && (!infoYG.Basic.syncInitSDK || syncInitSDKComplete))
            {
                iPlatform.InitStart();
                CallAction.CallIByAttribute(typeof(StartYGAttribute), typeof(YG2));
                _SDKEnabled = true;
                GetDataInvoke();
                iPlatform.InitComplete();
#if !UNITY_EDITOR
                Message("Init Game Success");
#endif
            }
        }

#if UNITY_EDITOR
        public static async void SyncInitialization()
        {
            if (infoYG.Basic.initDelaySimulation)
                await System.Threading.Tasks.Task.Delay(1000);
#else
        public static void SyncInitialization()
        {
#endif
            if (infoYG.Basic.syncInitSDK)
            {
                syncInitSDKComplete = true;

                if (!_SDKEnabled)
                {
                    if (awakePassed)
                    {
                        AwakeInit();
                    }
                    else
                    {
                        LoadNextScene();
                        return;
                    }

                    if (infoYG.Basic.loadSceneIfSDKLate)
                    {
                        SceneManager.sceneLoaded += LoadLastScene;
#if !UNITY_EDITOR
                        SceneManager.LoadScene(infoYG.Basic.loadSceneIndex);
#else
                        if (infoYG.Basic.initDelaySimulation)
                            SceneManager.LoadScene(infoYG.Basic.loadSceneIndex);
#endif
                        void LoadLastScene(Scene scene, LoadSceneMode mode)
                        {
                            StartInit();
                            SceneManager.sceneLoaded -= LoadLastScene;
                        }
                    }
                    else StartInit();
                }
                else LoadNextScene();
            }
            else LoadNextScene();

            void LoadNextScene()
            {
                if (infoYG.Basic.loadSceneIfSDKLate && infoYG.Basic.loadSceneIndex != 0)
                {
#if UNITY_EDITOR
                    if (infoYG.Basic.initDelaySimulation)
#endif
                        SceneManager.LoadScene(infoYG.Basic.loadSceneIndex);
                }
            }
        }

        public static void GetDataInvoke()
        {
            if (_SDKEnabled)
                onGetSDKData?.Invoke();
        }

        public static void Message(string message)
        {
#if UNITY_EDITOR
            if (infoYG.Basic.logInEditor)
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(1, true);
                System.Diagnostics.StackFrame frame = stackTrace.GetFrame(0);

                string fileName = frame.GetFileName();
                int lineNumber = frame.GetFileLineNumber();

                string dataPath = Application.dataPath.Replace("/", "\\");
                fileName = fileName.Replace(dataPath, string.Empty);
                fileName = "Assets" + fileName;

                Debug.Log($"<color=#ffffff>{message}</color>\n<color=#6b6b6b>{fileName}: {lineNumber}</color>");
            }
#else
            iPlatform.Message(message);
#endif
        }
    }
}