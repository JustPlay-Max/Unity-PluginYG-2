#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Serialization;

namespace YG.Insides
{
    public partial class PlatformInfo
    {
        [HeaderYG(LocalProdLangs.header)]
        [Platform("YandexGames"), Tooltip(LocalProdLangs.gameIdTooltip)]
        public string localProdGameId;

        [Platform("YandexGames"), Tooltip(LocalProdLangs.portTooltip)]
        public int localProdPort = 8080;

        [Platform("YandexGames"), Tooltip(LocalProdLangs.useCspTooltip)]
        public bool localProdUseCsp = true;

    }
}
#endif
