using YG.Utils.OpenURL;

namespace YG
{
    public partial interface IPlatformsYG2
    {
        void GetAllGamesInit() { }
        void OnURLDefineDomain(string url) { }
        GameInfo GetGameByID(int appID)
        {
            for (int i = 0; i < YG2.allGames.Length; i++)
            {
                if (YG2.allGames[i].appID == appID)
                    return YG2.allGames[i];
            }
            return null;
        }
    }
}