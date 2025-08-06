#if YandexGamesPlatform_yg
using System.Runtime.InteropServices;
using UnityEngine;
using YG.Utils.OpenURL;

namespace YG
{
    public partial class PlatformYG2 : IPlatformsYG2
    {
        class JsonAllGames
        {
            public int[] appID;
            public string[] title;
            public string[] url;
            public string[] coverURL;
            public string[] iconURL;
            public string developerURL;
        }

        [DllImport("__Internal")]
        private static extern string GetAllGames_js();

        public void GetAllGamesInit()
        {
            string jsonAllGamesStr = GetAllGames_js();
            if (jsonAllGamesStr == InfoYG.NO_DATA)
                return;

            JsonAllGames jsonAllGames = JsonUtility.FromJson<JsonAllGames>(jsonAllGamesStr);

            YG2.allGames = new GameInfo[jsonAllGames.appID.Length];

            for (int i = 0; i < jsonAllGames.appID.Length; i++)
            {
                YG2.allGames[i] = new GameInfo();
                YG2.allGames[i].appID = jsonAllGames.appID[i];
                YG2.allGames[i].title = jsonAllGames.title[i];
                YG2.allGames[i].url = jsonAllGames.url[i];
                YG2.allGames[i].coverURL = jsonAllGames.coverURL[i];
                YG2.allGames[i].iconURL = jsonAllGames.iconURL[i];
            }
            YG2.developerURL = jsonAllGames.developerURL;
        }


        public void OnURLDefineDomain(string url)
        {
            url = "https://yandex." + YG2.envir.domain + "/games/" + url;

            if (YG2.envir.domain != null && YG2.envir.domain != "")
            {
                YG2.OnURL(url);
            }
            else
            {
#if RU_YG2
                YG2.Message("OnURL_Yandex_DefineDomain: Домен не определен!");
#else
                YG2.Message("OnURL_Yandex_DefineDomain: Domain not defined!");
#endif
            }
        }
    }
}
#endif