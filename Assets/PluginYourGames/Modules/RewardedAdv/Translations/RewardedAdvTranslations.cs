namespace YG.Translations
{
    public class RewardedAdvTranslations
    {
#if RU_YG2
        public const string SkipInterAdvAfterRewardTooltip = "Пропустить следующий вызов обычной рекламы после получения вознаграждения. Такое требуют некоторые площадки, потому что показывать рекламу в одной сцене после того, как игрок уже посмотрел рекламу за вознаграждение - может быть нечестно.";
#else
        public const string SkipInterAdvAfterRewardTooltip = "Skip the next call of the regular advertisement after receiving the reward. This is required by some sites, because it may not be fair to show ads in one scene after the player has already watched the advertisement for a reward.";
#endif
    }
}