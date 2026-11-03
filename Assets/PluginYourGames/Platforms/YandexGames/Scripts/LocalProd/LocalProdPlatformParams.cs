#if UNITY_EDITOR
using UnityEngine;
using YG.Localization;

namespace YG.Insides
{
    public partial class PlatformInfo
    {
        [HeaderYG(LocalProdLocalization.header)]
        [Platform("YandexGames"), Tooltip(LocalProdLocalization.gameIdTooltip)]
        public string localProdGameId;

        [Platform("YandexGames"), Tooltip(LocalProdLocalization.portTooltip)]
        public int localProdPort = 8080;

        [Platform("YandexGames"), Tooltip(LocalProdLocalization.useCspTooltip)]
        public bool localProdUseCsp = true;

    }
}
#endif
