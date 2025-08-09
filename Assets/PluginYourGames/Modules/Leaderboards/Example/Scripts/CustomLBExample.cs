using UnityEngine;
using UnityEngine.UI;
using YG.Utils.LB; // !!!!!!!!

namespace YG.Example
{
    public class CustomLBExample : MonoBehaviour
    {
        public string technoName;
        public Text entriesText;

        private void OnEnable()
        {
            YG2.onGetLeaderboard += OnUpdateLB;
            YG2.GetLeaderboard(technoName);
        }

        private void OnDisable()
        {
            YG2.onGetLeaderboard -= OnUpdateLB;
        }

        private void OnUpdateLB(LBData lbData)
        {
            if (lbData.technoName == technoName)
            {
                entriesText.text = string.Empty;

                foreach (LBPlayerData player in lbData.players)
                {
                    entriesText.text += $"{player.rank}.  ";
                    entriesText.text += $"{player.name}:  ";
                    entriesText.text += $"{player.score}\n";
                }
            }
        }

    }
}