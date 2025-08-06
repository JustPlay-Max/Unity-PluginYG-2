using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class LBExample : MonoBehaviour
    {
        public LeaderboardYG leaderboardYG;
        public Text nameLbText;
        public InputField scoreLbInputField;
        public bool timeConverter;

        // Код для примера! Смена технического названия таблицы и её обновление в компоненте LeaderboardYG
        public void Start()
        {
            nameLbText.text = leaderboardYG.nameLB;
        }

        public void SetScore()
        {
            if (!timeConverter)
            {
                int score = int.Parse(scoreLbInputField.text);

                // Статический метод добавление нового рекорда
                YG2.SetLeaderboard(leaderboardYG.nameLB, score);

                // Метод добавление нового рекорда обращением к компоненту LeaderboardYG
                // leaderboardYG.SetLeaderboard(score);
            }
            else
            {
                scoreLbInputField.text = scoreLbInputField.text.Replace(".", ",");
                float score = float.Parse(scoreLbInputField.text);

                // Статический метод добавление нового рекорда конвертированного в time тип
                YG2.SetLBTimeConvert(leaderboardYG.nameLB, score);

                // Метод добавление нового рекорда обращением к компоненту LeaderboardYG
                // leaderboardYG.SetLBTimeConvert(score);
            }

            scoreLbInputField.text = string.Empty;
        }
    }
}