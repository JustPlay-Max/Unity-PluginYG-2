using System;
using UnityEngine;
using YG.Insides;
using YG.Translations;

namespace YG
{
    public partial class InfoYG
    {
        public RewardedAdvSettings RewardedAdv = new RewardedAdvSettings();

        [Serializable]
        public partial class RewardedAdvSettings
        {
            [Tooltip(RewardedAdvTranslations.SkipInterAdvAfterRewardTooltip)]
            public bool skipInterAdvAfterReward;

#if UNITY_EDITOR
            [SerializeField, LabelYG(Langs.advSimLabel)]
            private bool labelAdvSimLabel;
#endif
        }
    }
}