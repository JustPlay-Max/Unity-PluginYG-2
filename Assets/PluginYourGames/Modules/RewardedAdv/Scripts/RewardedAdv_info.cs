using System;
using UnityEngine;
using YG.Insides;

namespace YG
{
    public partial class InfoYG
    {
        public RewardedAdvSettings RewardedAdv = new RewardedAdvSettings();

        [Serializable]
        public partial class RewardedAdvSettings
        {
#if RU_YG2
            [Tooltip("Выдавать вознаграждение за просмотр рекламы только после закрытия рекламы?\n(true = после закрытия, false = сразу после того как таймер закончит свой отчёт)")]
#else
            [Tooltip("Give out rewards for viewing ads only after the ad is closed?\n(true = after closing, false = immediately after the timer finishes its report)")]
#endif
            public bool rewardedAfterClosing = true;
#if RU_YG2
            [Tooltip("Пропустить следующий вызов обычной рекламы после получения вознаграждения. Такое требуют некоторые площадки, потому что показывать рекламу в одной сцене после того, как игрок уже посмотрел рекламу за вознаграждение - может быть нечестно.")]
#else
            [Tooltip("Skip the next call of the regular advertisement after receiving the reward. This is required by some sites, because it may not be fair to show ads in one scene after the player has already watched the advertisement for a reward.")]
#endif
            public bool skipInterAdvAfterReward;

#if UNITY_EDITOR
            [SerializeField, LabelYG(Langs.advSimLabel)]
            private bool labelAdvSimLabel;
#endif
        }
    }
}