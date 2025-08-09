using System;
using System.Runtime.InteropServices;
using UnityEngine;
using YG.Utils.OpenURL;

namespace YG
{
    public partial class YG2
    {
        public static string developerURL;
        public static GameInfo[] allGames = new GameInfo[0];

        [InitYG]
        private static void GetAllGamesInit()
        {
#if UNITY_EDITOR
            allGames = infoYG.OpenURL.allGames;
            developerURL = infoYG.OpenURL.developerURL;
#else
            iPlatform.GetAllGamesInit();
#endif
        }

        public static GameInfo GetGameByID(int appID)
        {
#if UNITY_EDITOR
            return iPlatformNoRealization.GetGameByID(appID);
#else
            return iPlatform.GetGameByID(appID);
#endif
        }

        public static void OnDeveloperURL()
        {
            OnURL(developerURL);
        }

        public static void OnGameURL(int appID)
        {
            OnURL(GetGameByID(appID).url);
        }

        public static void OnURL(string url)
        {
            Message("URL Transition. url: " + url);
#if !PLATFORM_WEBGL || UNITY_EDITOR
            Application.OpenURL(url);
#else
            OnURLWebGL(url);
#endif
        }

#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern void OpenURL(string url);
        private static void OnURLWebGL(string url)
        {
            try
            {
                OpenURL(url);
            }
            catch (Exception error)
            {
#if RU_YG2
                Debug.LogError("Первый способ перехода по ссылке не удался! Ошибка:\n" + error + "\nВместо первого метода попробуем вызвать второй метод 'Application.OpenURL'");
#else
                Debug.LogError("The first method of following the link failed! Error:\n" + error + "\nInstead of the first method, let's try to call the second method 'Application.OpenURL'");
#endif
                Application.OpenURL(url);
            }
        }
#endif

        public static void OnURLDefineDomain(string url)
        {
            iPlatform.OnURLDefineDomain(url);
        }
    }
}

namespace YG.Utils.OpenURL
{
    [Serializable]
    public class GameInfo
    {
        public int appID = 0;
        public string title = string.Empty;
        public string url = string.Empty;
        public string coverURL = string.Empty;
        public string iconURL = string.Empty;
        public enum ImageURL { Icon, Cover }
    }
}