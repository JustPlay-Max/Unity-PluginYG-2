using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class ExampleRewardedAd : MonoBehaviour
    {
        public string idAdv;
        public Text rewardText;

        private int rewardCount = 0;

        private void Start()
        {
            rewardText.text = "0";
        }

        private void OnEnable()
        {
            YG2.onRewardAdv += Rewarded;
        }

        private void OnDisable()
        {
            YG2.onRewardAdv -= Rewarded;
        }

        private void Rewarded(string id)
        {
            if (id == idAdv)
            {
                SetReward();
            }
        }

        public void SetReward()
        {
            rewardCount += 1;
            rewardText.text = rewardCount.ToString();
        }

        // Пример вызова рекламы с получением вознаграждения без подписки на событие
        public void ShowRewardAdv_UseCallback()
        {
            YG2.RewardedAdvShow(idAdv, () =>
            {
                // Получение вознаграждения
                SetReward();
            });
        }
    }
}